using UnityEngine;

public class EnemyHitbox : BaseHitbox
{
    [Tooltip("Only enter positive x value since it will be flipped by attackFaceRight")]
    [SerializeField] Vector2 damageKnockback;
    [Tooltip("Used to apply knockback in correct direction")]
    [SerializeField] protected bool attackFaceRight;
    [SerializeField] float blockStunTime;
    [SerializeField] float blockKnockbackVelocity;
    [SerializeField] bool mustBlockUp;
    [SerializeField] bool mustBlockDown;
    public Color32 defaultActiveColor = new Color32(191, 49, 49, 152); // red
    public Color32 blockedInactiveColor = new Color32(161, 230, 230, 128); // teal

    /// <summary>
    /// Adjusts which way the knockback vector should be applied depending on assigned attack direction
    /// </summary>
    /// <returns></returns>
    public Vector2 GetKnockback()
    {
        return new Vector2(damageKnockback.x * (attackFaceRight ? 1 : -1), damageKnockback.y);
    }
    public float GetBlockStunTime() => blockStunTime;
    public float GetBlockKnockbackVelocity() => blockKnockbackVelocity;
    public bool GetMustBlockUp() => mustBlockUp;
    public bool GetMustBlockDown() => mustBlockDown;

    public void SetAttackFaceRight(bool faceRight)
    {
        attackFaceRight = faceRight;
    }

    public void SetColliderEnabledAndColor(bool colliderActive)
    {
        gameObject.GetComponent<Collider2D>().enabled = colliderActive;

        // change color
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (colliderActive)
            sr.color = defaultActiveColor;
        else
            sr.color = blockedInactiveColor;
    }

    public void RelayHitboxLandedToManager()
    {
        // disable all hitboxes

        if (this == null)
        {
            // object has already destroyed itself (like in projectile's case)
            return;
        }

        if (transform.parent == null)
        {
            Debug.LogWarning(name + " does not have a parent to disable all hitboxes");
            return;
        }

        if (transform.parent.TryGetComponent<EnemyHitboxManager>(out EnemyHitboxManager enemyHitboxManager))
            enemyHitboxManager.SetEnableAllHitboxes(false);
        else
            Debug.LogWarning(name + " does not have a parent EnemyHitboxManager to disable all hitboxes");
    }

    public virtual void GetParried()
    {
        if (transform.parent.TryGetComponent<EnemyHitboxManager>(out EnemyHitboxManager enemyHitboxManager))
            enemyHitboxManager.ChildHitboxParried(this);
        else
            Debug.LogWarning(name + " does not have a parent EnemyHitboxManager to process parry");
    }

    public virtual void GetBlocked()
    {

    }
}