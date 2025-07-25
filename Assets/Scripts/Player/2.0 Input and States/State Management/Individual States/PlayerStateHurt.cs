using UnityEngine;

public class PlayerStateHurt : PlayerBaseState
{
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
        stateManager.characterMover.SetVelocity(stateManager.hurtboxManager.GetIncomingHitbox().GetKnockback());

        // process damage, possibly causing death
        stateManager.playerHealth.DeductHealth(stateManager.hurtboxManager.GetIncomingHitbox().GetDamage());
    }

}
