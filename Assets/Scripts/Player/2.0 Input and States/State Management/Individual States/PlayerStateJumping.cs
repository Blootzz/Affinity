using UnityEngine;

public class PlayerStateJumping : PlayerStateFalling
{
    public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter(); // plays falling animation
        JumpByCharacterJumper();
        JumpEffects();
    }

    public virtual void JumpByCharacterJumper()
    {
        stateManager.characterJumper.BeginJumpAscent();
    }
    public virtual void JumpEffects()
    {
        stateManager.groundCheck.GetComponent<JumpLandDustFXManager>().EnableJumpDust();
    }

    public override void JumpCancel()
    {
        stateManager.characterJumper.BeginDescending();
    }

    public override void JumpStart()
    {
        base.JumpStart(); // double jump
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded)
            stateManager.SwitchState(new PlayerStateIdle(stateManager));
        //else do nothing, just keep jump state running
    }
}

public class PlayerStateDoubleJumping : PlayerStateJumping // inherits from PlayerStateFalling too
{
    public PlayerStateDoubleJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void JumpByCharacterJumper()
    {
        stateManager.characterJumper.BeginDoubleJumpAscent();
    }
}