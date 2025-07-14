using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {

    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.AnimatorSetBool("isBlocking", true);
    }

    public override void OnExit()
    {
        stateManager.playerAnimationManager.AnimatorSetBool("isBlocking", false);
    }

    public override void HorizontalAxis()
    {
        Debug.Log("implement directional block here");
    }
}
