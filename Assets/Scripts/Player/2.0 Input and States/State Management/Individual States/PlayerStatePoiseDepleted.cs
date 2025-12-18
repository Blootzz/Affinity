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
        StartFlashing();

        // no invulnerability
        // no knockback
        // poise damage already accounted for in PlayerStateBlocking

        // disable hitbox to prevent double hits
        EnemyHitbox enemyHitbox = stateManager.blockParryManager.GetIncomingEnemyHitbox();
        if (enemyHitbox != null) {
            enemyHitbox.RelayHitboxLandedToManager();
        }
        
        // no physics material change
        //// set physics material
        //physicsMaterialManager = stateManager.GetComponent<PhysicsMaterialManager>();
        //physicsMaterialManager.SetRbDamaged();
    }

    // same EndStateByAnimation() -> switches to idle

    public override void OnExit()
    {
        // do not mess with physicsMaterial like in PlayerStateHurt
        stateManager.characterMover.SetHorizontalMovementVelocity(0);
        stateManager.hurtboxManager.SetInvulnerability(false);
        StopFlashing();

        // give player some pitty poise
        stateManager.playerPoise.AddPoise(20);
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // do nothing
    }
    void StartFlashing()
    {
        stateManager.colorFlasher.StartRepeatingBlueFlash();
    }
    void StopFlashing()
    {
        stateManager.colorFlasher.EndBlueFlash();
    }
}
