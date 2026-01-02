using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Crouch")]
public class PlayerStateCrouching : PlayerStateIdle
{
    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUCrouch);
    }

    public override void VerticalAxis()
    {
        if (stateManager.GetLastSetYInput() >= 0)
            stateManager.SwitchState(stateManager.playerStateIdle);
    }
}
