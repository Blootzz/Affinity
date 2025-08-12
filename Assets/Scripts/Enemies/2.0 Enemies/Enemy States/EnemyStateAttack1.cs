using UnityEngine;

public class EnemyStateAttack1 : EnemyStateAttackBase
{
    
    public EnemyStateAttack1(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "Attack1"))
            stateManager.animator.Play("Attack1", -1, 0);
        else
            Debug.LogError("Does not contain animation \"Attack1\"");
    }
    public override void OnExit()
    {
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new EnemyStateIdle(stateManager));
    }
}
