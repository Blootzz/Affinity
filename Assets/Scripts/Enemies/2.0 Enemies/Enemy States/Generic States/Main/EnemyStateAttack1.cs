using UnityEngine;

public class EnemyStateAttack1 : EnemyStateAttackBase
{
    
    //public EnemyStateAttack1(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}

    /// <summary>
    /// Base.OnEnter() clears hitboxes
    /// This OnEnter() plays "Attack1" animation if found
    /// </summary>
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
        base.OnExit();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
