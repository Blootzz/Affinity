using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Hammer Soldier/Walk")]
public class HammerStateWalk : EnemyStateWalk
{
    public float walkSpeed;

    //public HammerStateWalk(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}

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
            stateManager.SwitchState(stateManager.stateAttack1);
        else
            ShuffleRandomly();
    }

    void ShuffleRandomly()
    {
        stateManager.characterMover.SetMoveSpeed(walkSpeed);

        // set direction as +/- 1
        stateManager.characterMover.SetHorizontalMovementVelocity((Random.value > 0.5f)? 1 : -1);
        stateManager.BeginStateUtilityTimer(0.5f);
    }
}
