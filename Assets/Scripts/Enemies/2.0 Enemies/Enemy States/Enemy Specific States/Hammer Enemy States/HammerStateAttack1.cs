using UnityEngine;

public class HammerStateAttack1 : EnemyStateAttack1
{
    public HammerStateAttack1(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new HammerStateAttack2(stateManager, 0));
    }

    public override void BeginLerpToPlayerByAnimation()
    {
        stateManager.ApproachPlayer(0.7f);
    }
}
