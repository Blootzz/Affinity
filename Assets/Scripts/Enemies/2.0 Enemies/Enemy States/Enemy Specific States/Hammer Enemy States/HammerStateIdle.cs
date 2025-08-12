using UnityEngine;


public class HammerStateIdle : EnemyStateIdle
{
    public HammerStateIdle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    /// <summary>
    /// called by Start() in EnemyStateManager
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnPlayerEnteredAttackZone()
    {
        stateManager.SwitchState(new HammerStateAttack1(stateManager));
    }
}
