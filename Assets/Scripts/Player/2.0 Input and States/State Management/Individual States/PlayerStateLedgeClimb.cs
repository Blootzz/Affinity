using UnityEngine;

public class PlayerStateLedgeClimb : PlayerBaseState
{
    // used to address ledge climb animation dropping the player at exactly the ledge when the animation ends farther
    Vector2 getupOffset = new Vector2(0.325f, 0);

    public PlayerStateLedgeClimb(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
        stateManager.characterMover.SetVelocity(Vector2.zero);

        // ledge climb sprites have pivot set to where the corner ought to be, so set position to the ledge
        // trying to turn on root motion breaks the character movement because the animator will always be trying to force the player to not let its transform move
        stateManager.characterMover.SetPosition(stateManager.ledgeGrabPos);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUClimbLedge);
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
        stateManager.characterMover.SetPosition(getupOffset * (stateManager.faceRight ? 1 : -1) + stateManager.ledgeGrabPos);
        Debug.Log("Setting position to: " + (getupOffset * (stateManager.faceRight ? 1 : -1)) + stateManager.ledgeGrabPos);
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
