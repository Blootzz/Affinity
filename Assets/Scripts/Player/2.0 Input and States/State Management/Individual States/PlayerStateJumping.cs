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

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded)
            stateManager.SwitchState(new PlayerStateIdle(stateManager));
        //else do nothing, just keep jump state running
    }
}
