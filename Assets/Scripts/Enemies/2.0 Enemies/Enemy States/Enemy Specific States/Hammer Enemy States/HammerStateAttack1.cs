using UnityEngine;

public class HammerStateAttack1 : EnemyStateAttack1
{
    public HammerStateAttack1(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new HammerStateAttack2(stateManager));
    }
}
