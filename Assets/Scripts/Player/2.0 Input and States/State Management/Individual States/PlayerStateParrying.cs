using UnityEngine;

public class PlayerStateParrying : PlayerStateBlocking
{
    bool wasParrySuccessful = false;

    public PlayerStateParrying(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // restart animation so that parry window re-opens in animation
        stateManager.playerAnimationManager.PlayAnimationFromStart(stateManager.playerAnimationManager.Parry);
        wasParrySuccessful = false; // setting this to false just in case it was somehow left as true

        stateManager.GetComponent<PhysicsMaterialManager>().SetRbHighFriction();
    }
    public override void OnExit()
    {
        // in case player is hit out of parry animation or something crazy happens
        if (wasParrySuccessful == false)
            Debug.Log("Deduct Poise Here");

        stateManager.blockParryManager.ClearIsParryWindowOpen();
        stateManager.blockParryManager.SetEnableBlockers(false, false);

        stateManager.GetComponent<PhysicsMaterialManager>().SetRbZeroFrictionBounce();
    }

    /// <summary>
    /// Evaluates if successful parry. Called by ProcessBlockerHit in PlayerStateBlocking
    /// </summary>
    public override void BlockSuccessful()
    {
        if (stateManager.blockParryManager.GetIsParryWindowOpen())
        {
            wasParrySuccessful = true;
            stateManager.blockParryManager.CreateVisualEffect(stateManager.faceRight, true);

            // add hitbox damage to poise
            stateManager.playerPoise.AddPoise(stateManager.blockParryManager.GetIncomingEnemyHitbox().GetDamage());

            // disable hitbox until enemy re-enables it
            stateManager.blockParryManager.DisableHitboxCollider();
        }
        //else
        //    Debug.Log("Failed Parry");

    }

    public override void Attack()
    {
        // restart to chain successful parries
        if (wasParrySuccessful)
            OnEnter();
    }
    public override void JumpStart()
    {
        if (wasParrySuccessful)
            base.JumpStart();
    }

    public override void HorizontalAxis()
    {
        if (wasParrySuccessful)
            base.HorizontalAxis();
    }// do nothing

    public override void VerticalAxis()
    {
        if (wasParrySuccessful)
        {
            // mostly copied from blocking state (minus animations)

            bool isInputHoldingUp = false;

            if (stateManager.GetLastSetYInput() > 0)
                isInputHoldingUp = true;

            // handle animations
            if (isInputHoldingUp)
            {
                // consider resuming upper parry animation here
                isBlockingUp = true;
            }
            else
            {
                // consider resuming lower parry animation here
                isBlockingUp = false;
            }

            // handle blockers
            // if isInputHoldingUp == false, enable lower, disable upper
            stateManager.blockParryManager.SetEnableBlockers(!isInputHoldingUp, isInputHoldingUp);
        }
    }// do nothing
    public override void BlockCancel()
    {
    }// do nothing

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }
}
