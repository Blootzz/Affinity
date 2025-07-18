using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        VerticalAxis(); // read vertical axis to determine which blocker to enable
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Block);
    }

    public override void OnExit()
    {
        stateManager.blockParryManager.SetEnableBlockers(false, false);
    }

    public override void HorizontalAxis()
    {
        stateManager.FlipIfNecessary();
    }

    public override void VerticalAxis()
    {
        bool isInputHoldingUp = false;

        if (stateManager.GetLastSetYInput() > 0)
            isInputHoldingUp = true;

        // if isInputHoldingUp == false, enable lower, disable upper
        stateManager.blockParryManager.SetEnableBlockers(!isInputHoldingUp, isInputHoldingUp);
    }

    public override void BlockCancel()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void Parry()
    {
        stateManager.SwitchState(new PlayerStateParrying(stateManager));
    }

    public override void ProcessBlockerHit()
    {
        // reference stateManager.blockParryManager
        // reference BlockParryCollider.CreateVisualEffect
        Debug.Log("evaluate block result here");
    }
}
