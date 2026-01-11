using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public event Action<bool> OnGroundedChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    [SerializeField] bool isGrounded;
    public bool IsGrounded => isGrounded;

    BoxCollider2D myCollider;
    List<Collider2D> overlapResults = new List<Collider2D>();

    [Tooltip("WARNING: including Physics Colliders layer will cause this to falsely detect player body in lower LineCast")]
    [SerializeField] LayerMask DetectionLayerMask;
    [Header("Wall Detection Points")]
    [SerializeField] Transform wallPointA;
    [SerializeField] Transform wallPointB;
    [Header("Floor Detection Points")]
    [SerializeField] Transform floorPointAFront;
    [SerializeField] Transform floorPointBFront;
    [SerializeField] Transform floorPointABack;
    [SerializeField] Transform floorPointBBack;

    private void Start()
    {
        isGrounded = EvaluateFloorCheck();
    }

    private void Update()
    {
        print("EvaluateFloorCheck: " + EvaluateFloorCheck());
        if (!isGrounded)
        {
            if (EvaluateFloorCheck())
            {
                // in the air and floor check was found
                FoundGround();
                return;
            }
            // in the air and floor was not found
            return;
        }

        // else IsGrounded is true. search for when player leaves ground entirely
        if (!EvaluateFloorCheck())
            LeftGround();
    }

    /// <summary>
    ///  returns true if either pair of floor points returns true on LineCast
    /// </summary>
    bool EvaluateFloorCheck()
    {
        RaycastHit2D hitFloor1 = Physics2D.Linecast(floorPointAFront.position, floorPointBFront.position, DetectionLayerMask);
        if (hitFloor1.collider != null)
            return true;

        RaycastHit2D hitFloor2 = Physics2D.Linecast(floorPointABack.position, floorPointBBack.position, DetectionLayerMask);
        if (hitFloor2.collider != null)
            return true;

        return false;

    }

    private void FoundGround()
    {
        //print("entering: " + collision.name);

        if (DidWeActuallyJustFindAVerticalWall())
        {
            //print("GroundCheck, False positive - just a vertical wall");
            return;
        }

        //print("entered ground: "+collision.gameObject.layer);

        isGrounded = true;
        OnGroundedChanged?.Invoke(isGrounded);
    }

    private void LeftGround()
    {
        //print("exiting: " + collision.name);

        // evaluate if there really is nothing in GroundCheck
        //if (IsGroundCheckStillOccupied())
        //{
        //    return;
        //}

        //print("Calling exit event");

        isGrounded = false;
        OnGroundedChanged?.Invoke(isGrounded);
    }

    //bool IsGroundCheckStillOccupied()
    //{
    //    overlapResults.Clear();
    //    if (myCollider.Overlap(overlapResults) != 0)
    //    {
    //        foreach (Collider2D collision in overlapResults)
    //        {
    //            //print("overlapResults contains: " + collision.gameObject.name);
    //            // if object matches DetectionLayerMask, return true
    //            if (((1 << collision.gameObject.layer) & DetectionLayerMask) != 0)
    //                return true;
    //        }
    //    }
    //    return false;
    //}

    /// <summary>
    /// Returns true if the wall LineCast hits and the floor LineCast doesn't
    /// </summary>
    bool DidWeActuallyJustFindAVerticalWall()
    {
        RaycastHit2D hitWall = Physics2D.Linecast(wallPointA.position, wallPointB.position, DetectionLayerMask);
        RaycastHit2D hitFloor = Physics2D.Linecast(floorPointAFront.position, floorPointBFront.position, DetectionLayerMask);
        if (hitWall.collider != null && hitFloor.collider == null)
            return true;

        //print("Floor linecast hit: " + hitFloor.collider.name);
        return false;
    }
}
