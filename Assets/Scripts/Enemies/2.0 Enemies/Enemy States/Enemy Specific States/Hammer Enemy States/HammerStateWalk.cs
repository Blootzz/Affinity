using UnityEngine;

public class HammerStateWalk : EnemyStateWalk
{
    public HammerStateWalk(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ShuffleRandomly();
    }

    public override void OnExit()
    {
        base.OnExit();
        stateManager.characterMover.SetHorizontalMovementVelocity(0);
    }

    public override void OnStateUtilityTimerEnd()
    {
        if (Random.value > 0.5f)
            stateManager.SwitchState(new HammerStateAttack1(stateManager));
        else
            ShuffleRandomly();
    }

    void ShuffleRandomly()
    {
        stateManager.characterMover.SetMoveSpeed(stateManager.walkSpeed);

        // set direction as +/- 1
        stateManager.characterMover.SetHorizontalMovementVelocity((Random.value > 0.5f)? 1 : -1);
        stateManager.BeginStateUtilityTimer(0.5f);
    }
}
