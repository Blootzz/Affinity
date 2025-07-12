using UnityEngine;

public class PlayerStateRunning : PlayerBaseState
{
    public PlayerStateRunning(PlayerStateManager newStateManager, float xInput) : base(newStateManager)
    {
        WASD(new Vector2(xInput, 0));
    }

    public override void WASD(Vector2 xyInput)
    {
        stateManager.characterMover.SetVelocity(new Vector2(xyInput.x, 0));
    }
}
