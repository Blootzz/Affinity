using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateManager stateManager;

    // Constructor sets stateManager
    public EnemyBaseState(EnemyStateManager newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public virtual void OnEnter()
    {
    }
    public virtual void OnExit()
    {
    }
    public virtual void EndStateByAnimation()
    {
    }
}
