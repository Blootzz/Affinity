using UnityEngine;

public class PlayerStateJumping : PlayerStateFalling
{
    public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter(); // plays falling animation
        stateManager.characterJumper.BeginJumpAscent();
    }

    public override void JumpCancel()
    {
        stateManager.characterJumper.BeginDescending();
    }

    public override void JumpStart()
    {
        //Debug.Log("TESTING Jump while in jump state");
        //OnEnter();
    }
}
