using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] Transform floorPointAFront; // previousFrontPos is just this but from the last update step
    [SerializeField] Transform floorPointABack; // previousFrontPos is just this but from the last update step
    [SerializeField] Transform floorPointBFront;
    [SerializeField] Transform floorPointBBack;

    Vector2 previousFrontPos; // used to ensure frame doesn't skip A to B Linecasts, records Front A position

    private void Start()
    {
        previousFrontPos = floorPointAFront.position;
        isGrounded = EvaluateFloorCheck();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            if (!EvaluateFloorCheck())
            {
                //print("leaving ground");
                ToggleIsGrounded();
            }
        }
        else
        {
            if (EvaluateFloorCheck() && !DidWeActuallyJustFindAVerticalWall())
            {
                //print("Entering ground");
                ToggleIsGrounded();
            }
        }

        // update previousPointBPos so that next comparison will use point from previous update, so colliders can't be skipped
        previousFrontPos = floorPointAFront.position;

        // physics step will happen after this loop
    }

    /// <summary>
    /// Called on FixedUpdate and will return true every frame player is grounded
    /// </summary>
    /// <returns> true if either pair of floor points returns true on LineCast</returns>
    bool EvaluateFloorCheck()
    {
        //print("previousFrontPos: "+previousFrontPos);
        //print("-> floorPointBFront: " +  floorPointBFront.position);
        RaycastHit2D hitFloorFront = Physics2D.Linecast(previousFrontPos, floorPointBFront.position, DetectionLayerMask);
        if (hitFloorFront.collider != null)
            return true;

        //print("-> floorPointBBack: " +  floorPointBBack.position);
        // uses floorPointABack to prevent turnaround error off wall jump
        RaycastHit2D hitFloorBack = Physics2D.Linecast(floorPointABack.position, floorPointBBack.position, DetectionLayerMask);
        if (hitFloorBack.collider != null)
            return true;

        return false;

    }

    void ToggleIsGrounded()
    {
        isGrounded = !isGrounded;
        OnGroundedChanged?.Invoke(isGrounded);
    }

    /// <summary>
    /// Returns true if the wall LineCast hits and the floor LineCast doesn't
    /// </summary>
    bool DidWeActuallyJustFindAVerticalWall()
    {
        RaycastHit2D hitWall = Physics2D.Linecast(wallPointA.position, wallPointB.position, DetectionLayerMask);
        RaycastHit2D hitFloor = Physics2D.Linecast(floorPointABack.position, floorPointBBack.position, DetectionLayerMask);
        if (hitWall.collider != null && hitFloor.collider == null)
            return true;

        //print("Floor linecast hit: " + hitFloor.collider.name);
        return false;
    }
}
