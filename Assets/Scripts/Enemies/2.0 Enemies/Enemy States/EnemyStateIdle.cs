using UnityEngine;

public class EnemyStateIdle : EnemyBaseState
{
    
    public EnemyStateIdle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    /// <summary>
    /// Plays "Idle" animation if found
    /// </summary>
    public override void OnEnter()
    {
        if (AnimatorHasClip(stateManager.animator, "Idle"))
            stateManager.animator.Play("Idle");
        else
            Debug.LogError("Does not contain animation \"Idle\"");
    }

    public virtual void DoNextAction()
    {
        Debug.LogWarning("No next action out of Idle");
    }
}
