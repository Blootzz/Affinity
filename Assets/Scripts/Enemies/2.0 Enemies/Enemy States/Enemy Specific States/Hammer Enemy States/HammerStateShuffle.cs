using UnityEngine;

public class HammerStateShuffle : EnemyStateShuffle
{
    public HammerStateShuffle(EnemyStateManager newStateManager) : base(newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnStateUtilityTimerEnd()
    {
        stateManager.SwitchState(new HammerStateAttack1(stateManager));        
    }
}
