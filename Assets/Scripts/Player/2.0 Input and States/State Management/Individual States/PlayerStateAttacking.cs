using UnityEngine;

public class PlayerStateAttacking : PlayerBaseState
{
    [SerializeField] float attackDamage = 20;

    public PlayerStateAttacking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerHitbox.SetDamage(attackDamage);
        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.StraightAttack);
    }

    public override void OnExit()
    {
        // hitbox should be disabled by animation, but include this here in case animation is disrupted
        DisableHitbox();
    }

    void DisableHitbox()
    {
        stateManager.playerHitbox.gameObject.SetActive(false);
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }
}
