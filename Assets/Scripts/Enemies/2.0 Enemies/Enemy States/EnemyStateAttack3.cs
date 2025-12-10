using UnityEngine;

public class EnemyStateAttack3 : EnemyStateAttackBase
{
    
    //public EnemyStateAttack3(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}
    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "Attack3"))
            stateManager.animator.Play("Attack3", -1, 0);
        else
            Debug.LogError("Does not contain animation \"Attack3\"");
    }
    public override void OnExit()
    {
        base.OnExit(); // ends slide towards player if not already taken care of
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
