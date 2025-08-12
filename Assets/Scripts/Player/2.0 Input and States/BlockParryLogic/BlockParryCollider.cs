using UnityEngine;

public class BlockParryCollider : MonoBehaviour
{
    BlockParryManager blockParryManager;
    GameObject effectSpawnPoint;

    private void Awake()
    {
        blockParryManager = transform.parent.GetComponent<BlockParryManager>();
        effectSpawnPoint = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // only process EnemyHitboxes
        if (collision.gameObject.TryGetComponent(out EnemyHitbox enemyHitbox))
        {
            print("BlockCollider");
            blockParryManager.FireBlockerHitEvent(enemyHitbox, effectSpawnPoint.transform.position);
        }
    }

}
