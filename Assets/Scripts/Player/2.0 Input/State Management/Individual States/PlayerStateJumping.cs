using UnityEngine;

public class PlayerStateJumping : PlayerBaseState
{
    public PlayerStateJumping(PlayerStateManager newStateManager) : base(newStateManager)
    {
        
    }

    public override void OnEnter()
    {
        stateManager.characterJumper.BeginJumpAscent();
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUFalling);
        // Plays AnselmFalling or AnselmFallingUnarmed animation
        // both have transitions into their extended falling versions
    }
    public override void OnExit()
    {

    }

    public override void HorizontalAxis(float xInput)
    {
        stateManager.characterMover.SetSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetHorizontalVelocity(xInput);
        if (stateManager.characterMover.FlipResult(stateManager.faceRight))
            stateManager.faceRight = !stateManager.faceRight;
    }

    public override void JumpCancel()
    {
        stateManager.characterJumper.BeginDescending();
    }

    public override void JumpStart()
    {
        Debug.Log("TESTING Jump while in jump state");
        OnEnter();
    }
}
