using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Jumping")]
public class PlayerStateJumping : PlayerStateFalling
{
    //public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

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

        // snap to wall slide similar to logic in FallingApexReached()
        if (stateManager.wallJumpCheck.GetIsInWall() && stateManager.GetLastSetXInput() != 0)
            stateManager.SwitchState(stateManager.playerStateWallSlide);
    }

    public override void JumpStart()
    {
        // if jump is pressed while in Jump state, defer to Fall state double jump check
        base.JumpStart(); // double jump
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded)
            stateManager.SwitchState(stateManager.playerStateIdle);
        //else do nothing, just keep jump state running
    }
}
