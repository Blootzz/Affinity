using UnityEngine;

public class BlockParryCollider : MonoBehaviour
{
    BlockParryManager blockParryManager;

    private void Awake()
    {
        blockParryManager = transform.parent.GetComponent<BlockParryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // only process EnemyHitboxes
        if (collision.gameObject.TryGetComponent(out EnemyHitbox enemyHitbox))
        {
            blockParryManager.FireBlockerHitEvent(enemyHitbox);
        }

    }
}
