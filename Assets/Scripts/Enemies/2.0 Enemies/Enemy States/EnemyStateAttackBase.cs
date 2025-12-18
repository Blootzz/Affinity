using UnityEngine;

public class EnemyStateAttackBase : EnemyBaseState
{
    //public EnemyStateAttackBase(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}

    /// <summary>
    /// Base: Enables all colliders for this enemy
    /// and sets rb to kinematic
    /// </summary>
    public override void OnEnter()
    {
        stateManager.gameObject.GetComponentInChildren<EnemyHitboxManager>().SetEnableAllHitboxes(true);
        stateManager.gameObject.GetComponentInChildren<EnemyHitboxManager>().SetHitboxAttackFaceRight(stateManager.facePlayer.GetFaceRight());

        stateManager.facePlayer.OneTimeCheck();
        stateManager.facePlayer.SetEnableAutomaticFlip(false);

        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
    }

    /// <summary>
    /// Ends SlideTowardPlayer if necessary
    /// </summary>
    public override void OnExit()
    {
        base.OnExit(); // does nothing as of writing this

        if (stateManager.GetComponent<SlideTowardPlayer>() != null)
            EndLerpToPlayerByAnimation();

        stateManager.facePlayer.SetEnableAutomaticFlip(true);
        EnemyCheckFlipAtEndOfAttack();

        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
    }

    public virtual void EnemyCheckFlipAtEndOfAttack()
    {
        stateManager.facePlayer.OneTimeCheck();
    }
}
