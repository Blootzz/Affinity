using UnityEngine;

public class BlockParryCollider : MonoBehaviour
{
    BlockParryManager blockParryManager;
    [SerializeField] ParticleSystem blockParticles;
    [SerializeField] GameObject blockWaveEffect;

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

    public void CreateVisualEffect()
    {
        //Vector2 angleVector = new Vector2(collision.gameObject.transform.position.x - localPos.x, collision.gameObject.transform.position.y - localPos.y);
        //float angleDeg = 180 / Mathf.PI * Mathf.Atan(angleVector.y / angleVector.x);

        Instantiate(blockWaveEffect, Vector3.zero, Quaternion.Euler(new Vector3(0, 0/*thePlayer.faceRight? 0:180*/, 0 /*angleDeg*/)), this.transform);

    }

}
