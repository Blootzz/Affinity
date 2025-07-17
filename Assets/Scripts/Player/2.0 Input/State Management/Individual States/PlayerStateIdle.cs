using UnityEngine;

public class PlayerStateIdle : PlayerBaseState
{
    public PlayerStateIdle(PlayerStateManager newStateManager) : base(newStateManager)
    { }

    public override void OnEnter()
    {
        if (stateManager.GetLastBlockInput())
        {
            stateManager.SwitchState(new PlayerStateBlocking(stateManager));
            return;
        }
        if (stateManager.GetLastSetXInput() != 0)
        {
            stateManager.SwitchState(new PlayerStateRunning(stateManager));
            return; // exit this state
        }

        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.DynamicIdle);
    }

    public override void HorizontalAxis()
    {
        stateManager.SwitchState(new PlayerStateRunning(stateManager));
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }

    public override void BlockStart()
    {
        stateManager.SwitchState(new PlayerStateBlocking(stateManager));
    }
}
