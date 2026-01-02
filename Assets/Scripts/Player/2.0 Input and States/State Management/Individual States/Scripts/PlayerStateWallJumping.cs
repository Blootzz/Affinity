using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/WallJumping")]
public class PlayerStateWallJumping : PlayerBaseState
{
    //public PlayerStateWallJumping(PlayerStateManager stateManager) : base(stateManager)
    //{
    //}

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

    public override void WallJumpCheckEntered()
    {
        stateManager.SwitchState(stateManager.playerStateWallSlide);
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.playerStateFalling);
    }

    public override void JumpStart()
    {
        if (stateManager.characterJumper.CheckIfDoubleJumpIsPossible())
            stateManager.SwitchState(stateManager.playerStateDoubleJumping);
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // nothing - avoids cancelling wall jump state and going straight into falling state
    }
}