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
        stateManager.gameObject.GetComponentInChildren<EnemyHitboxManager>().SetEnableAllHitboxes(true);
    }

    /// <summary>
    /// Ends SlideTowardPlayer if necessary
    /// </summary>
    public override void OnExit()
    {
        base.OnExit(); // does nothing as of writing this

        if (stateManager.GetComponent<SlideTowardPlayer>() != null)
            EndLerpToPlayerByAnimation();
    }

}
