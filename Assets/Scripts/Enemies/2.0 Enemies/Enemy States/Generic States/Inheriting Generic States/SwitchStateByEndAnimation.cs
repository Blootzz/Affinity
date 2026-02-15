using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic States/Inheriting Generic States/SwitchStateByEndAnimation")]
public class SwitchStateByEndAnimation : EnemyBaseState
{
    [SerializeField] EnemyBaseState nextState;

    [Header("Optional Aggro Logic")]
    [Tooltip("Should this check if enemy is aggro?")]
    [SerializeField] bool checkEnemyAggro = false;
    [Tooltip("State to enter if the enemy isn't aggro")]
    [SerializeField] EnemyBaseState noAggroState;

    public override void EndStateByAnimation()
    {
        //Debug.Log(stateManager.name + " end state by animation");

        stateManager.repeatStateCounter = 0;
        if (!checkEnemyAggro)
        {
            stateManager.SwitchState(nextState);
            return;
        }

        // use aggroCheck
        if (stateManager.isAggro)
        {
            //Debug.Log("isAggro = false, switching to nextState");
            stateManager.SwitchState(nextState);
        }
        else
        {
            //Debug.Log("isAggro = false, switching to noAggroState");
            stateManager.SwitchState(noAggroState);
        }
    }
}
