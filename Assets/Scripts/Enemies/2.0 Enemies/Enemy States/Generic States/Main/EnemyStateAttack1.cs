using UnityEngine;

public class EnemyStateAttack1 : EnemyStateAttackBase
{
    
    // animations handled by EnemyBaseState

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
