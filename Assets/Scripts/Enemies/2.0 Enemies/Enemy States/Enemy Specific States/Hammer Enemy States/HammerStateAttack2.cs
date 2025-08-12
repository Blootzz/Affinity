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
            stateManager.SwitchState(new EnemyStateAttack2(stateManager));
        }
        else
            stateManager.SwitchState(new EnemyStateAttack3(stateManager));
    }
}
