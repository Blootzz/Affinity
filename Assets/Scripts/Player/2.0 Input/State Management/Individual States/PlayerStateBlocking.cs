using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.Block);
    }

    public override void OnExit()
    {
    }

    public override void HorizontalAxis()
    {
        Debug.Log("implement directional block here");
    }

    public override void BlockCancel()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }

    public override void Parry()
    {
        stateManager.SwitchState(new PlayerStateParrying(stateManager));
    }

    public override void ProcessBlockerHit()
    {
        // reference stateManager.blockParryManager
        // reference BlockParryCollider.CreateVisualEffect
        Debug.Log("evaluate block result here");
    }
}
