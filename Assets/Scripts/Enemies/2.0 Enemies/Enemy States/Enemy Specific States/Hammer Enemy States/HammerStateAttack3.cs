using UnityEngine;

public class HammerStateAttack3 : EnemyStateAttack3
{

    public HammerStateAttack3(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

}
