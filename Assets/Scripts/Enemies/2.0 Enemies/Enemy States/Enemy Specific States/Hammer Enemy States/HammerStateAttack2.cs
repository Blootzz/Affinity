using UnityEngine;

public class HammerStateAttack2 : EnemyStateAttack2
{
    int attackRepeatLimit = 1;
    int attackRepeatCounter = 0;

    public HammerStateAttack2(EnemyStateManager newStateManager, int startingAttackCount) : base(newStateManager)
    {
        this.stateManager = newStateManager;
        attackRepeatCounter = startingAttackCount;
    }

    public override void EndStateByAnimation()
    {
        if (attackRepeatCounter < attackRepeatLimit)
        {
            attackRepeatCounter++;
            stateManager.SwitchState(new HammerStateAttack2(stateManager, attackRepeatCounter));
        }
        else
            stateManager.SwitchState(new HammerStateAttack3(stateManager));
    }
}
