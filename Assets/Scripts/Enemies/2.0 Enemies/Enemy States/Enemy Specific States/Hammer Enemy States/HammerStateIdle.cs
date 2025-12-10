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
        DoNextAction();
    }

    public override void OnPlayerEnteredAttackZone()
    {
        stateManager.SwitchState(new HammerStateAttack1(stateManager));
    }

    // 30% chance to skip shuffle state and go straight into attacking
    public override void DoNextAction()
    {
        Debug.Log("Check isAggro in Idle: "+stateManager.isAggro);

        if (stateManager.isAggro)
        {
            float random = Random.value;
            if (random < 0.3f)
                DoRandomAttack();
            else
                stateManager.SwitchState(new HammerStateWalk(stateManager));
        }
        else
        {
            // do nothing, wait to be aggro'd
        }
    }

    void DoRandomAttack()
    {
        int randInt = Random.Range(1, 4); // generates int 1-3

        switch (randInt)
        {
            case 1:
                stateManager.SwitchState(new HammerStateAttack1(stateManager));
                break;
            case 2:
                // does random number of HammerStateAttack2
                stateManager.SwitchState(new HammerStateAttack2(stateManager, Random.Range(1,3)));
                break;
            case 3:
                stateManager.SwitchState(new HammerStateAttack3(stateManager));
                break;
            default:
                Debug.LogError("No designated state to do next");
                break;
        }
    }
}
