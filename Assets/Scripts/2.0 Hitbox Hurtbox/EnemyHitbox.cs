using UnityEngine;

public class EnemyHitbox : BaseHitbox
{
    [Tooltip("Only enter positive x value since it will be flipped by attackFaceRight")]
    [SerializeField] Vector2 damageKnockback;
    [SerializeField] bool attackFaceRight;
    [SerializeField] float blockStunTime;
    [SerializeField] float blockKnockbackVelocity;
    [SerializeField] bool mustBlockUp;
    [SerializeField] bool mustBlockDown;

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
}