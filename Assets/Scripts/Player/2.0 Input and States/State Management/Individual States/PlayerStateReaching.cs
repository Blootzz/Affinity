using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Reaching")]
public class PlayerStateReaching : PlayerStateFalling
{
    //public PlayerStateReaching(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUReach);
        // both have transitions into their extended falling versions
        HorizontalAxis();
    
        // enable LedgeGrab Check
        stateManager.EnableLedgeGrabCheck(true);
    }

    public override void OnExit()
    {
        stateManager.EnableLedgeGrabCheck(false);
    }

    public override void LedgeGrabCanceled()
    {
        stateManager.SwitchState(stateManager.playerStateFalling);
    }

}
