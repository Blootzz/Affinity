using UnityEngine;

public class PlayerStateJumping : PlayerBaseState
{
    public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
        
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUFalling);
    }
    public override void OnExit()
    {

    }
}
