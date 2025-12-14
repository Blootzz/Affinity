using UnityEngine;

public class PlayerStateLedgeHang : PlayerBaseState
{
    public PlayerStateLedgeHang(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUHanging);
        stateManager.characterMover.SetRbType(RigidbodyType2D.Kinematic);
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
        if (stateManager.faceRight && stateManager.GetLastSetXInput() < 0 || stateManager.faceRight! && stateManager.GetLastSetXInput() > 0)
        {
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
            return;
        }

        // if player presses forward from ledge, do a getup animation and end the state by animation
        if (stateManager.faceRight && stateManager.GetLastSetXInput() > 0 || stateManager.faceRight! && stateManager.GetLastSetXInput() < 0)
        {
            stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUClimbLedge);
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
}
