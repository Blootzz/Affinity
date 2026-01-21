using UnityEngine;

public class EnemyStateWalk : EnemyBaseState
{
    //public EnemyStateWalk(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //}
    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "Walk"))
            stateManager.animator.Play("Walk", -1, 0);
        else
            Debug.LogError("Does not contain animation \"Walk\"");
    }
}
