using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Falling")]
public class PlayerStateFalling : PlayerBaseState
{
    //public PlayerStateFalling(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUFalling);
        // Plays AnselmFalling or AnselmFallingUnarmed animation
        // both have transitions into their extended falling versions
        HorizontalAxis();
        if (stateManager.GetLastLedgeGrabInput())
            LedgeGrabStarted();
    }

    public override void HorizontalAxis()
    {
        stateManager.characterMover.SetMoveSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetHorizontalMovementVelocity(stateManager.GetLastSetXInput());
        //if (stateManager.characterMover.FlipResult(stateManager.faceRight))
        //    stateManager.faceRight = !stateManager.faceRight;
        stateManager.FlipIfNecessary();
    }

    public override void LedgeGrabStarted()
    {
        stateManager.SwitchState(stateManager.playerStateReaching);
    }

    public override void JumpStart()
    {
        if (stateManager.characterJumper.CheckIfDoubleJumpIsPossible())
            stateManager.SwitchState(stateManager.playerStateDoubleJumping);
    }

    public override void SHORYUKEN()
    {
        stateManager.SwitchState(stateManager.playerStateSHORYUKEN);
    }

    public override void WallCheckEntered()
    {
        stateManager.SwitchState(stateManager.playerStateWallSlide);
    }

    public override void FallingApexReached()
    {
        if (stateManager.wallCheck.GetIsInWall())
        {
            stateManager.SwitchState(stateManager.playerStateWallSlide);
        }
    }

}
