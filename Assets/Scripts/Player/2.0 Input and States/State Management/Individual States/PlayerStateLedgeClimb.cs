using UnityEngine;

public class PlayerStateLedgeClimb : PlayerBaseState
{
    public PlayerStateLedgeClimb(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
        stateManager.characterMover.SetVelocity(Vector2.zero);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUClimbLedge);
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        // do nothing
    }
}
