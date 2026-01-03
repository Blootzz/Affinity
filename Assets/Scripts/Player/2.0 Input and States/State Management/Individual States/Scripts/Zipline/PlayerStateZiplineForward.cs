using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Zipline/Zipline Forward")]
public class PlayerStateZiplineForward : PlayerBaseState
{
    [Tooltip("Sum of this offset and the ledge position equals good player position while facing right")]
    [SerializeField] Vector2 positionOffset = new Vector2(-.2755f, -0.851f);

    public override void OnEnter()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
        stateManager.characterMover.SetVelocity(Vector2.zero);

        // adjust offset depending on facing right or left
        Vector2 adjustedOffset = new Vector2(positionOffset.x * (stateManager.faceRight ? 1 : -1), positionOffset.y);
        // assign offset to player
        stateManager.characterMover.SetRBPosition(stateManager.ledgeGrabPos + adjustedOffset);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.ZiplineForward);
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(stateManager.playerStateJumping);
    }

    public override void HorizontalAxis()
    {

    }

    public override void VerticalAxis()
    {
        // if player presses down, simply start falling
        if (stateManager.GetLastSetYInput() < 0)
        {
            stateManager.SwitchState(stateManager.playerStateFalling);
            return;
        }
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {

    }
}
