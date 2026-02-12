using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Projectile : EnemyHitbox
{
    Rigidbody2D rb;

    [SerializeField] float speed = 0.2f;

    [SerializeField] Vector3 angle = new Vector3(1, 0);

    //[SerializeField] bool parried = false;
    [SerializeField] float reflectMultiplier = 1.5f;
    [SerializeField] bool destroyOnBlock = true;
    [SerializeField] bool destroyOnParry = false;

    [SerializeField] bool isParried = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        print("found collision with: "+collision.gameObject.name);
    }

    public override void GetBlocked()
    {
        if (destroyOnBlock)
            Destroy(this.gameObject);
    }

    public override void GetParried()
    {
        if (destroyOnParry)
        {
            Destroy(this.gameObject);
            return;
        }

        // become a playerHitbox that can hit enemies
        PlayerHitbox playerHitbox = gameObject.AddComponent<PlayerHitbox>();
        playerHitbox.SetDamage(damage * reflectMultiplier);

        isParried = true;

        GetComponent<BaseProjectileBehaviour>().Reflect();
    }

    public void SetFaceRight(bool faceRight)
    {
        attackFaceRight = faceRight;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetAngle(Vector2 newAngle)
    {
        angle = newAngle;
    }

    public bool GetAttackFaceRight() { return attackFaceRight; }
    public float GetSpeed() { return speed; }
    public Vector3 GetAngle() { return angle; }
    public bool GetIsParried() { return isParried; }

}// Projectile
