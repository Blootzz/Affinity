using UnityEngine;

public class PlayerStateRunning : PlayerBaseState
{
    // needs a special constructor because HorizontalAxis can't get called by OnEnter
    public PlayerStateRunning(PlayerStateManager newStateManager, float xInput) : base(newStateManager)
    {
        HorizontalAxis(xInput);
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorURun);
    }

    public override void HorizontalAxis(float xInput)
    {
        stateManager.characterMover.SetSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetVelocity(new Vector2(xInput, 0));
        if (stateManager.characterMover.FlipResult(stateManager.faceRight))
            stateManager.faceRight = !stateManager.faceRight;
    }

    public override void Jump()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }
}
