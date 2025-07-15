using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Block);
    }

    public override void OnExit()
    {
    }

    public override void HorizontalAxis()
    {
        Debug.Log("implement directional block here");
    }
}
