using UnityEngine;

public class PlayerStateGuitar : PlayerBaseState
{
    public PlayerStateGuitar(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.SwitchActionMap("Guitar");
    }

    public override void OnExit()
    {
        stateManager.SwitchActionMap("Basic");
    }
}
