using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/SHORYUKEN")]
public class PlayerStateSHORYUKEN : PlayerStateAttacking
{
    [SerializeField] float shoryuDamage = 30;
    [SerializeField] Vector2 shoryuVelocity = new Vector2(2, 12);

    //public PlayerStateSHORYUKEN(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

    public override void OnEnter()
    {
        stateManager.playerHitbox.SetDamage(shoryuDamage);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUSHORYUKEN);
        DoShoryu();
    }

    void DoShoryu()
    {
        stateManager.characterMover.SetVelocity(new Vector2(shoryuVelocity.x * (stateManager.faceRight ? 1 : -1), shoryuVelocity.y));
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.playerStateFalling);
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // ignore
    }
}