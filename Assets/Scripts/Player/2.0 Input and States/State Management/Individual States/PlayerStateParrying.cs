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
    }
    public override void OnExit()
    {
        // in case player is hit out of parry animation or something crazy happens
        if (wasParrySuccessful == false)
            Debug.Log("Deduct Poise Here");

        stateManager.blockParryManager.ClearIsParryWindowOpen();
    }

    /// <summary>
    /// Called by ProcessBlockerHit in PlayerStateBlocking
    /// </summary>
    public override void BlockSuccessful()
    {
        if (stateManager.blockParryManager.GetIsParryWindowOpen())
        {
            stateManager.blockParryManager.CreateVisualEffect(stateManager.faceRight, true);
            wasParrySuccessful = true;
            stateManager.blockParryManager.GetIncomingEnemyHitbox().gameObject.SetActive(false); // disable hitbox until enemy re-enables it
        }
        //else
        //    Debug.Log("Failed Parry");

    }

    public override void Parry()
    {
        // restart to chain successful parries
        if (wasParrySuccessful)
            OnEnter();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void HorizontalAxis()
    {
    }// do nothing

    public override void VerticalAxis()
    {
    }// do nothing
    public override void BlockCancel()
    {
    }// do nothing
}
