using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "States/Player/Running")]
public class PlayerStateRunning : PlayerStateIdle
{
    public event Action<bool> StartedRunningEvent; // listened to by FootstepController

    public override void OnEnter()
    {
        HorizontalAxis();
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorURun);
        StartedRunningEvent?.Invoke(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        stateManager.characterMover.SetHorizontalMovementVelocity(0);
        StartedRunningEvent?.Invoke(false);
    }

    public override void HorizontalAxis()
    {
        // pass in speed/velocity
        stateManager.characterMover.SetMoveSpeed(stateManager.runSpeed);
        stateManager.characterMover.SetHorizontalMovementVelocity(stateManager.GetLastSetXInput());

        if (stateManager.GetLastSetXInput() == 0)
        {
            stateManager.SwitchState(stateManager.playerStateIdle);
            return;
        }

        stateManager.FlipIfNecessary();

        //if (stateManager.characterMover.FlipResult(stateManager.faceRight))
        //    stateManager.faceRight = !stateManager.faceRight;
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(stateManager.playerStateJumping);
    }

    //public override void ProcessGroundCheckEvent(bool isGrounded)
    //{
    //    //if (isGrounded == false)
    //    //{
    //    //    Debug.Log("switching from running to falling");
    //    //    stateManager.SwitchState(new PlayerStateFalling(stateManager));
    //    //}
    //    //else
    //    //    Debug.LogWarning("Running state just received isGrounded is now true???\n" +
    //    //        "This is probably due to being spawned in airbourne with no state");
    //}

    public override void BlockStart()
    {
        base.BlockStart();
        stateManager.characterMover.SetHorizontalMovementVelocity(0);
    }
}
