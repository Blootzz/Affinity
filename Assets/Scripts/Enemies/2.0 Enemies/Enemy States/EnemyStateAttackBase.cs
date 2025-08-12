using UnityEngine;

public class EnemyStateAttackBase : EnemyBaseState
{
    public EnemyStateAttackBase(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    /// <summary>
    /// Base: Enables all colliders for this enemy
    /// </summary>
    public override void OnEnter()
    {
        Debug.Log("Starting new enemy attack");
        stateManager.gameObject.GetComponentInChildren<EnemyHitboxManager>(true).EnableAllColliders();
    }
}
