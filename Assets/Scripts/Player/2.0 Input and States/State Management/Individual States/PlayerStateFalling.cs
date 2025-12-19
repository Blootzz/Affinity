using UnityEngine;

public class PlayerStateFalling : PlayerBaseState
{
    public PlayerStateFalling(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUFalling);
        // Plays AnselmFalling or AnselmFallingUnarmed animation
        // both have transitions into their extended falling versions
        HorizontalAxis();
        if (stateManager.GetLastInteractInput())
            InteractStart();
    }

    public override void HorizontalAxis()
    {
        stateManager.characterMover.SetMoveSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetHorizontalMovementVelocity(stateManager.GetLastSetXInput());
        //if (stateManager.characterMover.FlipResult(stateManager.faceRight))
        //    stateManager.faceRight = !stateManager.faceRight;
        stateManager.FlipIfNecessary();
    }

    public override void InteractStart()
    {
        stateManager.SwitchState(new PlayerStateReaching(stateManager));
    }

    public override void JumpStart()
    {
        if (stateManager.characterJumper.CheckIfDoubleJumpIsPossible())
            stateManager.SwitchState(new PlayerStateDoubleJumping(stateManager));
    }

    public override void SHORYUKEN()
    {
        stateManager.SwitchState(new PlayerStateSHORYUKEN(stateManager));
    }

    public override void WallCheckEntered()
    {
        stateManager.SwitchState(new PlayerStateWallSlide(stateManager));
    }

    public override void FallingApexReached()
    {
        if (stateManager.wallCheck.GetIsInWall())
            stateManager.SwitchState(new PlayerStateWallSlide(stateManager));
    }

}
