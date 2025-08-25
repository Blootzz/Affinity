using UnityEngine;

public class EnemyStateShuffle : EnemyBaseState
{
    public EnemyStateShuffle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }
}
