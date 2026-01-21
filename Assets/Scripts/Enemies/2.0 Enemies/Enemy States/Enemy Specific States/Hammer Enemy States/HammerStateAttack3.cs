using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Hammer Soldier/Attack3")]
public class HammerStateAttack3 : EnemyStateAttack3
{
    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
