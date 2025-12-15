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
        base.InteractStart();

        // enable LedgeGrab Check
        stateManager.EnableLedgeGrabCheck(true);
    }
    public override void InteractCancel()
    {
        base.InteractCancel();

        stateManager.EnableLedgeGrabCheck(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        InteractCancel();
    }
}
