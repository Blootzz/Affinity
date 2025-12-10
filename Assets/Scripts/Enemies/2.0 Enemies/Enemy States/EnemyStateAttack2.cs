using UnityEngine;

public class EnemyStateAttack2 : EnemyStateAttackBase
{
    
    //public EnemyStateAttack2(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}
    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "Attack2"))
            stateManager.animator.Play("Attack2", -1, 0);
        else
            Debug.LogError("Does not contain animation \"Attack2\"");
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
