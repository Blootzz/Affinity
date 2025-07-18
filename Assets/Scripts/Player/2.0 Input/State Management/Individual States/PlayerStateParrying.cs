using UnityEngine;

public class PlayerStateParrying : PlayerBaseState
{
    bool wasParrySuccessful = false;

    public PlayerStateParrying(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Parry);
        wasParrySuccessful = false; // setting this to false just in case it was somehow left as true
    }
    public override void OnExit()
    {
        // in case player is hit out of parry animation or something crazy happens
        if (wasParrySuccessful == false)
            Debug.Log("Deduct Poise Here");

        stateManager.blockParryManager.ClearIsParryWindowOpen();
    }

    public override void ProcessBlockerHit()
    {
        if (stateManager.blockParryManager.GetIsParryWindowOpen())
        {
            Debug.Log("Successful parry");
            stateManager.blockParryManager.CreateVisualEffect(stateManager.faceRight, true);
            wasParrySuccessful = true;
        }
        else
            Debug.Log("Failed Parry");
        // set wasParrySuccessful = true when a successful parry is performed
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
}
