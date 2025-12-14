using UnityEngine;
using System;

public class WallCheck2 : MonoBehaviour
{
    public event Action<bool> OnWallCollisionChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    public bool IsInWall { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("entering: " + collision.name);
        IsInWall = true;
        OnWallCollisionChanged?.Invoke(IsInWall);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //print("exiting: " + collision.name);
        IsInWall = false;
        OnWallCollisionChanged?.Invoke(IsInWall);
    }
}
