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
            blockParryManager.FireBlockerHitEvent(enemyHitbox, effectSpawnPoint.transform.position);
        }
    }

    // most hitboxes should be triggers, but this would be used for things like the training ball
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // only process EnemyHitboxes
        if (collision.gameObject.TryGetComponent(out EnemyHitbox enemyHitbox))
        {
            blockParryManager.FireBlockerHitEvent(enemyHitbox, effectSpawnPoint.transform.position);
        }
    }

}
