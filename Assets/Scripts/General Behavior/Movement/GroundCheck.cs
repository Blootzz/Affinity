using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public event Action<bool> OnGroundedChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    public bool IsGrounded { get; private set; }

    BoxCollider2D myCollider;
    List<Collider2D> overlapResults = new List<Collider2D>();

    [Tooltip("WARNING: including Physics Colliders layer will cause this to falsely detect player body in lower LineCast")]
    [SerializeField] LayerMask DetectionLayerMask;
    [Header("Wall Detection Points")]
    [SerializeField] Transform wallPointA;
    [SerializeField] Transform wallPointB;
    [Header("Floor Detection Points")]
    [SerializeField] Transform floorPointA;
    [SerializeField] Transform floorPointB;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("entering: " + collision.name);

        //// if incoming object is NOT on layer in this mask (result == 0)
        //if (((1 << collision.gameObject.layer) & DetectionLayerMask) == 0)
        //    return;

        if (DidWeActuallyJustFindAVerticalWall())
        {
            //print("GroundCheck, False positive - just a vertical wall");
            return;
        }

        //print("entered ground");

        IsGrounded = true;
        OnGroundedChanged?.Invoke(IsGrounded);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("exiting: " + collision.name);

        //// if incoming object is NOT on layer in this mask (result == 0)
        //if (((1 << collision.gameObject.layer) & DetectionLayerMask) == 0)
        //    return;

        // evaluate if there really is nothing in GroundCheck
        if (IsGroundCheckStillOccupied())
        {
            return;
        }

        //print("Calling exit event");

        IsGrounded = false;
        OnGroundedChanged?.Invoke(IsGrounded);
    }

    bool IsGroundCheckStillOccupied()
    {
        overlapResults.Clear();
        if (myCollider.Overlap(overlapResults) != 0)
        {
            foreach (Collider2D collision in overlapResults)
            {
                //print("overlapResults contains: " + collision.gameObject.name);
                // if object matches DetectionLayerMask, return true
                if (((1 << collision.gameObject.layer) & DetectionLayerMask) != 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if the wall LineCast hits and the floor LineCast doesn't
    /// </summary>
    bool DidWeActuallyJustFindAVerticalWall()
    {
        RaycastHit2D hitWall = Physics2D.Linecast(wallPointA.position, wallPointB.position, DetectionLayerMask);
        RaycastHit2D hitFloor = Physics2D.Linecast(floorPointA.position, floorPointB.position, DetectionLayerMask);
        if (hitWall.collider != null && hitFloor.collider == null)
            return true;

        //print("Floor linecast hit: " + hitFloor.collider.name);
        return false;
    }
}
