using UnityEngine;
using System;

public class WallCheck2 : MonoBehaviour
{
    public event Action<bool> OnWallCollisionChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    [SerializeField] bool IsInWall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("entering: " + collision.name);
        IsInWall = true;
        OnWallCollisionChanged?.Invoke(IsInWall);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("exiting: " + collision.name);
        IsInWall = false;
        OnWallCollisionChanged?.Invoke(IsInWall);
    }

    public bool GetIsInWall() => IsInWall;
}
