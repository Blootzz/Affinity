using UnityEngine;

public class HammerStateAttack2 : EnemyStateAttack2
{
    int attackRepeatLimit = 1;
    int attackRepeatCounter = 0;

    public HammerStateAttack2(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public override void EndStateByAnimation()
    {
        if (attackRepeatCounter < attackRepeatLimit)
        {
            attackRepeatCounter++;
            stateManager.animator.Play("Attack2", -1, 0);
        }
        else
            stateManager.SwitchState(new EnemyStateAttack3(stateManager));
    }
}
