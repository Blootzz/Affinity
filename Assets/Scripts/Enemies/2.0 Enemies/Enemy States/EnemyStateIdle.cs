using UnityEngine;

public class EnemyStateIdle : EnemyBaseState
{
    
    public EnemyStateIdle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public override void OnEnter()
    {

    }
    public override void OnExit()
    {
    }
}
