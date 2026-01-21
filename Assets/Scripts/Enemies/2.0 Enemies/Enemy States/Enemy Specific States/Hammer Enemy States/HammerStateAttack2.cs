using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Hammer Soldier/Attack2")]
public class HammerStateAttack2 : EnemyStateAttack2
{
    int attackRepeatLimit = 1;
    float lerpToPlayerStrength = 0.3f;

    public override void OnEnter()
    {
        base.OnEnter();
        //attackRepeatCounter = startingAttackCount; // don't need this if ScOb data is preserved
    }

    public override void EndStateByAnimation()
    {
        if (stateManager.repeatStateCounter < attackRepeatLimit)
        {
            stateManager.repeatStateCounter++;
            stateManager.SwitchState(stateManager.stateAction2);
        }
        else
            stateManager.SwitchState(stateManager.stateAction3);
    }

    public override void BeginLerpToPlayerByAnimation()
    {
        stateManager.ApproachPlayer(lerpToPlayerStrength);
    }
}
