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

    [Header("Wall Detection Points")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("entering: " + collision.name);

        if (DidWeActuallyJustFindAVerticalWall())
            return;

        IsGrounded = true;
        OnGroundedChanged?.Invoke(IsGrounded);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("exiting: " + collision.name);

        // evaluate if there really is nothing in GroundCheck
        if (!IsGroundCheckStillOccupied())
            return;

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

    bool DidWeActuallyJustFindAVerticalWall()
    {
        RaycastHit2D hit = Physics2D.Linecast(pointA.position, pointB.position);
        if (hit.collider != null)
            return true;
        return false;
    }
}
