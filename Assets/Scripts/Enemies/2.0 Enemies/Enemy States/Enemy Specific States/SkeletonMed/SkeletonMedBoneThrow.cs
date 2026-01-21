using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/SkeletonMed/ThrowBone")]
public class SkeletonMedBoneThrow : SwitchStateByEndAnimation
{
    [SerializeField] GameObject bonePrefab;
    [SerializeField] Vector2 spawnOffset;

    public override void OnEnter()
    {
        base.OnEnter();
        Vector2 adjustedSpawnPos = new Vector2(spawnOffset.x * (stateManager.facePlayer.GetFaceRight() ? -1 : 1), spawnOffset.y);
        Projectile flyingBone = Instantiate(bonePrefab, adjustedSpawnPos, Quaternion.identity).GetComponent<Projectile>();
        flyingBone.SetAttackFaceRight(stateManager.facePlayer.GetFaceRight());
    }
}
