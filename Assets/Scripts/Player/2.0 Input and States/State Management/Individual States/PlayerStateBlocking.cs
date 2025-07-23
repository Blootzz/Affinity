using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // read vertical axis to determine which blocker to enable and what animations to play
        VerticalAxis();
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

        // handle animations
        if (isInputHoldingUp)
        {
            stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.BlockUp);
        }
        else
        {
            stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Block);
        }
        
        // handle blockers
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
        stateManager.blockParryManager.CreateVisualEffect(stateManager.faceRight, false);
        Debug.Log("evaluate block result here");
    }
}
