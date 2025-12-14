using UnityEngine;
using System;

public class PlayerStateHurt : PlayerBaseState
{
    PhysicsMaterialManager physicsMaterialManager;

    public PlayerStateHurt(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // invulnerability
        stateManager.hurtboxManager.SetInvulnerability(true);
        
        // animation
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUStunned);

        EnemyHitbox enemyHitbox = stateManager.hurtboxManager.GetIncomingEnemyHitbox();
        // do knockback
        stateManager.characterMover.SetVelocity(enemyHitbox.GetKnockback());

        // process damage, possibly causing death
        stateManager.playerHealth.DeductHealth(enemyHitbox.GetDamage());

        // disable hitbox to prevent double hits
        enemyHitbox.RelayHitboxLandedToManager();
        
        // set physics material
        physicsMaterialManager = stateManager.GetComponent<PhysicsMaterialManager>();
        physicsMaterialManager.SetRbDamaged();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetHorizontalMovementVelocity(0);
        physicsMaterialManager.SetRbZeroFrictionBounce();
        stateManager.hurtboxManager.SetInvulnerability(false);
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // do nothing
    }
}
