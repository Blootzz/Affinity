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

    private void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("entering: " + collision.name);
        IsGrounded = true;
        OnGroundedChanged?.Invoke(IsGrounded);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("exiting: " + collision.name);

        // evaluate if there really is nothing in GroundCheck
        overlapResults.Clear();
        if (myCollider.Overlap(overlapResults) != 0)
            return;

        IsGrounded = false;
        OnGroundedChanged?.Invoke(IsGrounded);
    }
}
