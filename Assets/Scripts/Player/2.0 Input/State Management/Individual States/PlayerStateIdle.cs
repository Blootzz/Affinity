using UnityEngine;

public class PlayerStateIdle : PlayerBaseState
{
    public PlayerStateIdle(PlayerStateManager newStateManager) : base(newStateManager)
    { }

    public override void HorizontalAxis(float xInput)
    {
        stateManager.SwitchState(new PlayerStateRunning(stateManager, xInput));
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }
}
