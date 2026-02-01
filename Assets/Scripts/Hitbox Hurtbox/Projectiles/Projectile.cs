using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Projectile : EnemyHitbox
{
    Rigidbody2D rb;
    public Rigidbody2D Rb;

    [SerializeField] float speed = 0.2f;
    public float Speed => speed;

    [SerializeField] Vector3 angle = new Vector3(1, 0);
    public Vector3 GetAngle => angle;

    public bool AttackFaceRight => attackFaceRight;
    
    //[SerializeField] bool parried = false;
    [SerializeField] float reflectMultiplier = 1.5f;

    public override void GetBlocked()
    {
        Destroy(this.gameObject);
    }

    public override void GetParried()
    {
        PlayerHitbox playerHitbox = gameObject.AddComponent<PlayerHitbox>();
        playerHitbox.SetDamage(damage * reflectMultiplier);

        //parried = true;
        // default behavior
        angle *= -1;
    }


}// Projectile
