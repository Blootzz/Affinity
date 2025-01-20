using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float damage = 10;
    [SerializeField]
    Vector2 knockback = new Vector2(2, 6);
    [SerializeField]
    float speed = 0.2f;
    [SerializeField]
    Vector3 angle = new Vector3(1, 0);
    [SerializeField]
    bool parried = false;
    float reflectMultiplier = 1.5f;

    public bool moveRight = false;

    public virtual void FixedUpdate()
    {
        transform.position += angle*speed*(moveRight ? 1:-1);
    }// basic linear movement

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherThing = collision.gameObject;
        if (!parried)
        {
            if (!otherThing.CompareTag("Enemy") && !otherThing.CompareTag("Ledge")) // Ensures projectiles will not interact with enemies
            {
                if (!otherThing.CompareTag("Blocker")) // logic for when player blocks a projectile called by Blocker.cs
                {
                    if (otherThing.CompareTag("Player"))
                    {
                        otherThing.GetComponent<Player>().TakeHit(damage, knockback, transform.position.x > otherThing.transform.position.x);
                    }
                    if (otherThing.CompareTag("Shield"))
                    {
                        otherThing.GetComponent<Shield>().BeginToComeBack();
                    }
                    Destroy(this.gameObject); // "THIS" IS NECESSARY. DOESN'T WORK WITHOUT "THIS" KEYWORD. VISUAL STUDIO IS LYING.
                }// not hitting blocker
            }// If not colliding with enemy
        }// normal not parried behavior
        else // parried behavior
        {
            if (otherThing.CompareTag("Enemy"))
            {
                //print(otherThing.GetComponent<Enemy>());
                otherThing.GetComponent<Enemy>().TakeDamage(damage * reflectMultiplier, 0.2f, 1f, knockback);
                return;
            }
            Destroy(this.gameObject);
        }// parried behavior

    }// OnTriggerEnter2D

    public virtual void GetBlocked()
    {
        Destroy(this.gameObject);
    }

    public virtual void GetParried()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player"); // becomes friendly
        parried = true;

        // default behavior
        angle *= -1;
    }

}// Projectile
