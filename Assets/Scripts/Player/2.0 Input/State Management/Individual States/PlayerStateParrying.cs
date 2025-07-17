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

    public override void ProcessBlockerHit(EnemyHitbox enemyHitbox, bool isParryWindowOpen)
    {
        Debug.Log("evaluate parry result here");
    }
}
