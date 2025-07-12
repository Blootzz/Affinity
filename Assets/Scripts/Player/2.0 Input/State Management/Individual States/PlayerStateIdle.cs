using UnityEngine;

public class PlayerStateIdle : PlayerBaseState
{
    public PlayerStateIdle(PlayerStateManager newStateManager) : base(newStateManager)
    { }

    public override void WASD(Vector2 xyInput)
    {
        stateManager.SwitchState(new PlayerStateRunning(stateManager, xyInput.x));
    }
}
