using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/SkeletonMed/ThrowBone")]
public class SkeletonMedBoneThrow : SwitchStateByEndAnimation
{
    [SerializeField] GameObject bonePrefab;
    [SerializeField] Vector2 spawnOffset;

    [Header("Random Throw Height (overrides spawnOffset)")]
    [SerializeField] bool isRandomHeightEnabled = false;
    [SerializeField] float highThrowSpawnY;
    [SerializeField] float lowThrowSpawnY;

    public override void MidAnimationEvent()
    {
        // establish spawn point based off manager position and faceRight. Include offset
        Vector3 objPosition = stateManager.gameObject.transform.position;
        float spawnPosX = objPosition.x + (spawnOffset.x * (stateManager.facePlayer.GetFaceRight() ? 1 : -1));
        float spawnPosY = objPosition.y + spawnOffset.y;

        // optional binary random height adjustment
        if (isRandomHeightEnabled)
        {
            if (Random.value < 0.5)
                spawnPosY = objPosition.y + lowThrowSpawnY;
            else
                spawnPosY = objPosition.y + highThrowSpawnY;
        }

        Vector2 adjustedSpawnPos = new Vector2(spawnPosX, spawnPosY);

        Projectile flyingBone = Instantiate(bonePrefab, adjustedSpawnPos, Quaternion.identity).GetComponent<Projectile>();
        flyingBone.SetAttackFaceRight(stateManager.facePlayer.GetFaceRight());
    }
}
