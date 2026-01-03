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

        // enter wall slide if directly facing wall
        EvaluateWallSlideOnHorizontalInput();
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

    public override void WallJumpCheckEntered()
    {
        // only if player is holding in towards wall
        if (stateManager.characterMover.GetYVelocity() < 0 && stateManager.GetLastSetXInput() != 0)
            stateManager.SwitchState(stateManager.playerStateWallSlide);
    }

    public override void FallingApexReached()
    {
        if (stateManager.wallJumpCheck.GetIsInWall() && stateManager.GetLastSetXInput() != 0)
            stateManager.SwitchState(stateManager.playerStateWallSlide);
    }

    public override void JumpCancel()
    {
        // check PlayerStateJumping if you're looking to debug this
    }

    void EvaluateWallSlideOnHorizontalInput()
    {
        // FallingApexReached logic: if WallCheck is in the wall and is holding in
        if (stateManager.wallJumpCheck.GetIsInWall() && stateManager.GetLastSetXInput() != 0 && stateManager.characterMover.GetYVelocity() < 0)
            stateManager.SwitchState(stateManager.playerStateWallSlide);
    }
}
