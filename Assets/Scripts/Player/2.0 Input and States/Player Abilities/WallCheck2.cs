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

public class PlayerStateWallSlide : PlayerBaseState
{
    [SerializeField] float slideDownVelocityY = -2;
    public PlayerStateWallSlide(PlayerStateManager stateManager) : base(stateManager)
    {
    }

    public override void OnEnter()
    {
        // animation has player looking opposite direction than faceRight
        // This is good because we want player to keep facing direction if we slide off
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUWallSlide);

        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
        SlideDown();
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
        stateManager.EnableLedgeGrabCheck(false);
    }

    /// <summary>
    /// Sets velocity to (0, slideDownVelocity)
    /// </summary>
    void SlideDown()
    {
        stateManager.characterMover.SetVelocity(new Vector2(0, slideDownVelocityY));
    }

    /// <summary>
    /// Detach from wall if horizontal input is away from wall
    /// </summary>
    public override void HorizontalAxis()
    {
        // player is still "facing" wall
        if (stateManager.GetLastSetXInput() < 0 && stateManager.faceRight || stateManager.GetLastSetXInput() > 0 && !stateManager.faceRight)
        {
            // flip to prevent falling state immediately recognize wall slide again
            stateManager.ForceFlip();
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
        }
    }

    /// <summary>
    /// Detach from wall if vertical input is down
    /// </summary>
    public override void VerticalAxis()
    {
        if (stateManager.GetLastSetYInput() < 0)
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
    }

    /// <summary>
    /// Make sure player can grab ledge if they just started sliding
    /// </summary>
    public override void InteractStart()
    {
        stateManager.EnableLedgeGrabCheck(true);
    }
    public override void InteractCancel()
    {
        stateManager.EnableLedgeGrabCheck(false);
    }

    /// <summary>
    /// if the wall was an overhang and just ran out
    /// </summary>
    public override void WallCheckExited()
    {
        stateManager.SwitchState(new PlayerStateFalling(stateManager));
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateWallJumping(stateManager));
    }

}

public class PlayerStateWallJumping : PlayerBaseState
{
    public PlayerStateWallJumping(PlayerStateManager stateManager) : base(stateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.ForceFlip();
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUWallJump);
        stateManager.characterJumper.BeginWallJumpAscent(stateManager.faceRight); // must be called after ForceFlip()
    }

    public override void OnExit()
    {
        //stateManager.characterMover.SetVelocityX(0);
    }

    public override void WallCheckEntered()
    {
        stateManager.SwitchState(new PlayerStateWallSlide(stateManager));
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateFalling(stateManager));
    }

}