using UnityEngine;

public class EnemyStateAttack2 : EnemyStateAttackBase
{
    // animations handled by base state

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
