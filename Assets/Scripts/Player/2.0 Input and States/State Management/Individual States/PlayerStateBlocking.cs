using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    bool isBlockingUp = false;
    bool persistBlockerUponExit = false;

    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // read vertical axis to determine which blocker to enable and what animations to play
        VerticalAxis();
        persistBlockerUponExit = false; // ensure false, probably not necessary
    }

    public override void OnExit()
    {
        if (!persistBlockerUponExit)
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
            isBlockingUp = true;
        }
        else
        {
            stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Block);
            isBlockingUp = false;
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
        persistBlockerUponExit = true;
        stateManager.SwitchState(new PlayerStateParrying(stateManager));
    }

    public override void ProcessBlockerHit()
    {
        if (isBlockingUp && stateManager.blockParryManager.GetIncomingEnemyHitbox().GetMustBlockDown())
        {
            BlockSuccessful();
            return;
        }
        if (!isBlockingUp && stateManager.blockParryManager.GetIncomingEnemyHitbox().GetMustBlockUp())
        {
            BlockSuccessful();
            return;
        }

        // if hitbox block direction is not specified
        BlockSuccessful();
    }

    public virtual void BlockSuccessful()
    {
        Debug.Log("Blocker calling Block Successful");
        // reference stateManager.blockParryManager
        stateManager.blockParryManager.CreateVisualEffect(stateManager.faceRight, false);
        stateManager.blockParryManager.GetIncomingEnemyHitbox().gameObject.SetActive(false); // disable hitbox until enemy re-enables it

    }
}
