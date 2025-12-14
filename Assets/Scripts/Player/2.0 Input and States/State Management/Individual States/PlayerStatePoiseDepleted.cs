using UnityEngine;
using System;

public class PlayerStatePoiseDepleted : PlayerStateHurt
{

    public PlayerStatePoiseDepleted(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // animation
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUPoiseDepleted);
        
        // no invulnerability
        // no knockback
        // poise damage already accounted for in PlayerStateBlocking

        // disable hitbox to prevent double hits
        EnemyHitbox enemyHitbox = stateManager.hurtboxManager.GetIncomingEnemyHitbox();
        enemyHitbox.RelayHitboxLandedToManager();
        
        // no physics material change
        //// set physics material
        //physicsMaterialManager = stateManager.GetComponent<PhysicsMaterialManager>();
        //physicsMaterialManager.SetRbDamaged();
    }

    // same EndStateByAnimation() -> switches to idle
    // OnExit same as PlayerStateHurt

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // do nothing
    }
}
