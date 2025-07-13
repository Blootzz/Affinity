using UnityEngine;

public class PlayerStateJumping : PlayerBaseState
{
    public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
        
    }

    public override void OnEnter()
    {
        stateManager.characterJumper.Jump();
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUFalling);
        // Plays AnselmFalling or AnselmFallingUnarmed animation
        // both have transitions into their extended falling versions
    }
    public override void OnExit()
    {

    }
}
