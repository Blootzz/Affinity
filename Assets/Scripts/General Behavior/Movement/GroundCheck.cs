using UnityEngine;
using System;

public class GroundCheck : MonoBehaviour
{
    public event Action<bool> OnGroundedChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    public bool IsGrounded { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("entering: " + collision.name);
        IsGrounded = true;
        OnGroundedChanged?.Invoke(IsGrounded);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print("exiting: " + collision.name);
        IsGrounded = false;
        OnGroundedChanged?.Invoke(IsGrounded);
    }
}
