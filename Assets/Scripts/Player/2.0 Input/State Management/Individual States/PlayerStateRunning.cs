using UnityEngine;

public class PlayerStateRunning : PlayerBaseState
{
    // needs a special constructor because HorizontalAxis can't get called by OnEnter
    public PlayerStateRunning(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        HorizontalAxis();
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorURun);
    }

    public override void HorizontalAxis()
    {
        // pass in speed/velocity
        stateManager.characterMover.SetSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetHorizontalVelocity(stateManager.GetLastSetXInput());

        if (stateManager.GetLastSetXInput() == 0)
        {
            stateManager.SwitchState(new PlayerStateIdle(stateManager));
            return;
        }

        stateManager.FlipIfNecessary();

        //if (stateManager.characterMover.FlipResult(stateManager.faceRight))
        //    stateManager.faceRight = !stateManager.faceRight;
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }

    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded == false)
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
        else
            Debug.LogWarning("Running state just received isGrounded is now true???\n" +
                "This is probably due to being spawned in airbourne with no state");
    }
}
