using UnityEngine;

public class PlayerStateLedgeHang : PlayerBaseState
{
    [Tooltip("Sum of this offset and the ledge position equals good player position while facing right")]
    [SerializeField] Vector2 positionOffset = new Vector2(-.2755f, -0.851f);

    public PlayerStateLedgeHang(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
        stateManager.characterMover.SetVelocity(Vector2.zero);

        // adjust offset depending on facing right or left
        Vector2 adjustedOffset = new Vector2(positionOffset.x * (stateManager.faceRight ? 1 : -1), positionOffset.y);
        // assign offset to player
        stateManager.characterMover.SetPosition(stateManager.ledgeGrabPos + adjustedOffset);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUHanging);
    }

    public override void OnExit()
    {
        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }

    public override void HorizontalAxis()
    {
        // if player selects the direction away from the ledge, simply start falling
        if ((stateManager.faceRight && stateManager.GetLastSetXInput() < 0) || (!stateManager.faceRight && stateManager.GetLastSetXInput() > 0))
        {
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
            return;
        }

        // if player presses forward from ledge, enter ledge climb state
        if (stateManager.faceRight && stateManager.GetLastSetXInput() > 0 || stateManager.faceRight! && stateManager.GetLastSetXInput() < 0)
        {
            stateManager.SwitchState(new PlayerStateLedgeClimb(stateManager));
            return;
        }

    }

    public override void VerticalAxis()
    {
        // if player presses down, simply start falling
        if (stateManager.GetLastSetYInput() < 0)
        {
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
            return;
        }
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {

    }
}
