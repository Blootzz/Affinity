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
        Debug.Log("Implement invulnerability here");
        
        // animation
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUStunned);

        // do knockback
        stateManager.characterMover.SetVelocity(stateManager.hurtboxManager.GetIncomingEnemyHitbox().GetKnockback());

        // set physics material
        physicsMaterialManager = stateManager.GetComponent<PhysicsMaterialManager>();
        physicsMaterialManager.SetRbPlayerDamaged();

        // process damage, possibly causing death
        stateManager.playerHealth.DeductHealth(stateManager.hurtboxManager.GetIncomingEnemyHitbox().GetDamage());
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetHorizontalVelocity(0);
        physicsMaterialManager.SetRbZeroFrictionBounce();
    }
}
