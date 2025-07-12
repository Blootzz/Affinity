using UnityEngine;

public class PlayerStateRunning : PlayerBaseState
{
    public PlayerStateRunning(PlayerStateManager newStateManager, float xInput) : base(newStateManager)
    {
        HorizontalAxis(xInput);
    }

    public override void HorizontalAxis(float xInput)
    {
        stateManager.characterMover.SetSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetVelocity(new Vector2(xInput, 0));
    }
}
