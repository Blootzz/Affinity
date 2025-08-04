using UnityEngine;

public class EnemyStateAttack1 : EnemyBaseState
{
    
    public EnemyStateAttack1(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public override void OnEnter()
    {
        stateManager.animator.Play("Attack1");
    }
    public override void OnExit()
    {
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new EnemyStateIdle(stateManager));
    }
}
