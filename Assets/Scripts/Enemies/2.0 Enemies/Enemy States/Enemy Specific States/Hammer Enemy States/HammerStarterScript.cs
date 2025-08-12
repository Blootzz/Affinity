using UnityEngine;

public class HammerStarterScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("using external script to assign HammerStateIdle. This is stupid but it is what it is");
        EnemyStateManager enemyStateManager = GetComponent<EnemyStateManager>();
        enemyStateManager.SwitchState(new HammerStateIdle(enemyStateManager));
    }
}
