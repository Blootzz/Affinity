using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : EnemyHitbox
{
    Rigidbody2D rb;
    [SerializeField]
    float speed = 0.2f;
    [SerializeField]
    Vector3 angle = new Vector3(1, 0);
    [SerializeField]
    bool parried = false;
    float reflectMultiplier = 1.5f;

    public virtual void FixedUpdate()
    {
        transform.position += angle * speed * Time.fixedDeltaTime * 50 * (attackFaceRight ? 1 : -1);
    }// basic linear movement

    public override void GetBlocked()
    {
        Destroy(this.gameObject);
    }

    public override void GetParried()
    {
        PlayerHitbox playerHitbox = gameObject.AddComponent<PlayerHitbox>();
        playerHitbox.SetDamage(damage * reflectMultiplier);

        parried = true;
        // default behavior
        angle *= -1;
    }

}// Projectile
