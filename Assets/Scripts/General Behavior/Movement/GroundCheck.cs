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

    [SerializeField] LayerMask LineCastLayerMask;
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

        if (DidWeActuallyJustFindAVerticalWall())
        {
            //print("GroundCheck, False positive - just a vertical wall");
            return;
        }

        IsGrounded = true;
        OnGroundedChanged?.Invoke(IsGrounded);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("exiting: " + collision.name);

        // evaluate if there really is nothing in GroundCheck
        if (IsGroundCheckStillOccupied())
        {
            //print("ground check is still occupied");
            return;
        }

        print("Calling exit event");

        IsGrounded = false;
        OnGroundedChanged?.Invoke(IsGrounded);
    }

    bool IsGroundCheckStillOccupied()
    {
        overlapResults.Clear();
        if (myCollider.Overlap(overlapResults) != 0)
            return true;
        return false;
    }

    /// <summary>
    /// Returns true if the wall LineCast hits and the floor LineCast doesn't
    /// </summary>
    bool DidWeActuallyJustFindAVerticalWall()
    {
        RaycastHit2D hitWall = Physics2D.Linecast(wallPointA.position, wallPointB.position, LineCastLayerMask);
        RaycastHit2D hitFloor = Physics2D.Linecast(floorPointA.position, floorPointB.position, LineCastLayerMask);
        if (hitWall.collider != null && hitFloor.collider == null)
            return true;

        //print("Floor linecast hit: " + hitFloor.collider.name);
        return false;
    }
}
