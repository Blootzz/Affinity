using UnityEngine;

public class PlayerStateParrying : PlayerBaseState
{
    public PlayerStateParrying(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Parry);
    }
    public override void OnExit()
    {
        // in case player is hit out of parry animation or something crazy happens
        stateManager.blockParryManager.ClearIsParryWindowOpen();
    }

    public override void ProcessBlockerHit(EnemyHitbox enemyHitbox, bool isParryWindowOpen)
    {
        Debug.Log("evaluate parry result here");
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }
}
