using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Idle")]
public class PlayerStateIdle : PlayerBaseState
{
    //public PlayerStateIdle(PlayerStateManager newStateManager) : base(newStateManager)
    //{ }

    public override void OnEnter()
    {
        if (stateManager.groundCheck.IsGrounded == false)
        {
            stateManager.SwitchState(stateManager.playerStateFalling);
            return;
        }
        if (stateManager.GetLastBlockInput())
        {
            stateManager.SwitchState(stateManager.playerStateBlocking);
            return;
        }
        if (stateManager.GetLastSetXInput() != 0)
        {
            stateManager.SwitchState(stateManager.playerStateRunning);
            return; // exit this state
        }
        if (stateManager.GetLastSetYInput() < 0)
        {
            stateManager.SwitchState(stateManager.playerStateCrouching);
            return; // exit this state
        }

        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.DynamicIdle);
        stateManager.GetComponent<PhysicsMaterialManager>().SetRbHighFriction();
    }

    public override void OnExit()
    {
        base.OnExit();
        stateManager.GetComponent<PhysicsMaterialManager>().SetRbZeroFrictionBounce();
    }

    public override void HorizontalAxis()
    {
        if (stateManager.GetLastSetXInput() != 0)
            stateManager.SwitchState(stateManager.playerStateRunning);
    }

    public override void VerticalAxis()
    {
        if (stateManager.GetLastSetYInput() < 0)
            stateManager.SwitchState(stateManager.playerStateCrouching);
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(stateManager.playerStateJumping);
    }

    public override void BlockStart()
    {
        stateManager.SwitchState(stateManager.playerStateBlocking);
    }

    public override void Attack()
    {
        stateManager.SwitchState(stateManager.playerStateAttacking);
    }

    public override void SHORYUKEN()
    {
        stateManager.SwitchState(stateManager.playerStateSHORYUKEN);
    }

    public override void OpenGuitar()
    {
        stateManager.SwitchState(stateManager.playerStateGuitar);
    }
}
