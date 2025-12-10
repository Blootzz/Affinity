using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Hammer Soldier/Attack3")]
public class HammerStateAttack3 : EnemyStateAttack3
{

    //public HammerStateAttack3(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}

    // can't lerp while animation manipulates transform. Causes unexpected behaviour where animation can't finish
    //public override void BeginLerpToPlayerByAnimation()
    //{
    //    stateManager.ApproachPlayer(1f);
    //}

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
