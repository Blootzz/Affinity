using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/SkeletonMed/ThrowBone")]
public class SkeletonMedBoneThrow : EnemyStateAttack2
{
    [SerializeField] GameObject bonePrefab;

    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "Attack2"))
            stateManager.animator.Play("Attack2", -1, 0);
        else
            Debug.LogError("Does not contain animation \"Attack2\"");
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(stateManager.stateIdle);
    }
}
