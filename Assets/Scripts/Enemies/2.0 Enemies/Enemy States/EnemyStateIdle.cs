using UnityEngine;

public class EnemyStateIdle : EnemyBaseState
{
    
    public EnemyStateIdle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public override void OnEnter()
    {
        if (AnimatorHasClip(stateManager.animator, "Idle"))
            stateManager.animator.Play("Idle");
        else
            Debug.LogError("Does not contain animation \"Idle\"");
    }
}
