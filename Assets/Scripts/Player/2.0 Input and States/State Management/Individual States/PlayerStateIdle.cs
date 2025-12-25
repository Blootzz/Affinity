using UnityEngine;

public class PlayerStateIdle : PlayerBaseState
{
    public PlayerStateIdle(PlayerStateManager newStateManager) : base(newStateManager)
    { }

    public override void OnEnter()
    {
        if (stateManager.groundCheck.IsGrounded == false)
        {
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
            return;
        }
        if (stateManager.GetLastBlockInput())
        {
            stateManager.SwitchState(new PlayerStateBlocking(stateManager));
            return;
        }
        if (stateManager.GetLastSetXInput() != 0)
        {
            stateManager.SwitchState(new PlayerStateRunning(stateManager));
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
        stateManager.SwitchState(new PlayerStateRunning(stateManager));
    }

    public override void JumpStart()
    {
        stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }

    public override void BlockStart()
    {
        stateManager.SwitchState(new PlayerStateBlocking(stateManager));
    }

    public override void Attack()
    {
        stateManager.SwitchState(new PlayerStateAttacking(stateManager));
    }

    public override void SHORYUKEN()
    {
        stateManager.SwitchState(new PlayerStateSHORYUKEN(stateManager));
    }

    public override void OpenGuitar()
    {
        stateManager.SwitchState(new PlayerStateGuitar(stateManager));
    }
}
