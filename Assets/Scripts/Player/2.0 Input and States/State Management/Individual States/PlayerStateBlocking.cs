using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    protected bool isBlockingUp = false;
    bool persistBlockerUponExit = false; // used if entering a new state that utilizes the blockers

    //public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

    public override void OnEnter()
    {
        // read vertical axis to determine which blocker to enable and what animations to play
        VerticalAxis();
        persistBlockerUponExit = false; // ensure false, probably not necessary

        stateManager.GetComponent<PhysicsMaterialManager>().SetRbHighFriction();
    }

    public override void OnExit()
    {
        if (!persistBlockerUponExit)
            stateManager.blockParryManager.SetEnableBlockers(false, false);
        stateManager.GetComponent<PhysicsMaterialManager>().SetRbZeroFrictionBounce();
    }

    public override void HorizontalAxis()
    {
        stateManager.FlipIfNecessary();
    }

    /// <summary>
    /// changes which blocker/animation activates. DOES NOT TAKE PLAYER OUT OF BLOCK STATE
    /// </summary>
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

    /// <summary>
    /// Switches state to idle when block button is released
    /// </summary>
    public override void BlockCancel()
    {
        stateManager.SwitchState(stateManager.playerStateIdle);
    }

    /// <summary>
    /// Switch state to Parry, persistBlockerUponExit = true;
    /// </summary>
    public override void Attack()
    {
        persistBlockerUponExit = true;
        stateManager.SwitchState(stateManager.playerStateParrying);
    }

    public override void ProcessBlockerHit()
    {
        if (stateManager.blockParryManager.GetIncomingEnemyHitbox().GetMustBlockUp())
        {
            if (isBlockingUp)
                BlockSuccessful();
            else
                BlockFailed();
            return;
        }
        if (stateManager.blockParryManager.GetIncomingEnemyHitbox().GetMustBlockDown())
        {
            if (isBlockingUp)
                BlockFailed();
            else
                BlockSuccessful();
            return;
        }

        // if hitbox block direction is not specified
        BlockSuccessful();
    }

    /// <summary>
    /// When not overridden, deducts poise, creates block effect, disables hitbox
    /// Switches state to BlockSlide then deducts Poise, which switches state to PoiseDepleted if applicable
    /// </summary>
    public virtual void BlockSuccessful()
    {
        // switch to block slide state
        stateManager.SwitchState(stateManager.playerStateBlockSlide);

        // deduct Poise, SWITCHES TO POISEDEPLETED if applicable
        stateManager.playerPoise.DeductPoise(stateManager.blockParryManager.GetIncomingEnemyHitbox().GetDamage());

        // notify blockParryManager of successful block
        stateManager.blockParryManager.OnSuccessfulBlock(stateManager.faceRight);
    }
    /// <summary>
    /// Called when player attempts to block but failed to block in the necessary vertical direction
    /// </summary>
    public virtual void BlockFailed()
    {
        Debug.Log("block failed");
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(stateManager.playerStateJumping);
    }
}
