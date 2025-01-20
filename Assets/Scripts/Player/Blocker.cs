using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    Player thePlayer;
    public ParticleSystem blockParticles;
    public GameObject blockWaveEffect;
    bool blockEffectCooldown = false;
    public Vector2 fixedPos = new Vector2(.3f, .3f);

    void Start()
    {
        thePlayer = gameObject.GetComponentInParent<Player>(); // fill reference to Player object
    }

    private void OnCollisionEnter2D(Collision2D collision) // interaction with non-triggers
    {
        //print("collided with: " + collision.gameObject.name+" Layer: "+collision.gameObject.layer);
        if (!blockEffectCooldown)
            CreateBlockEffects(collision);
        thePlayer.AddStamina(15);
    }

    private void OnTriggerEnter2D(Collider2D collision) // interaction with triggers
    {
        //print("triggered with: " + collision.gameObject.name);
        if (!blockEffectCooldown)
            CreateBlockEffects(collision);
        GameObject otherThing = collision.gameObject;
        if (otherThing.CompareTag("Projectile"))
        {
            if (thePlayer.parryWindowOpen)
            {
                otherThing.gameObject.GetComponent<Projectile>().GetParried();
                thePlayer.AddStamina(thePlayer.maxStamina);
            }
            else
            {
                otherThing.gameObject.GetComponent<Projectile>().GetBlocked(); // performs logic for projectiles to follow when blocked
                thePlayer.AddStamina(20);
            }
        }
    }

    void CreateBlockEffects(Collision2D collision) // interaction with non-triggers
    {
        Vector2 localPos = new Vector2((thePlayer.faceRight ? 1:-1) * fixedPos.x + thePlayer.transform.position.x, fixedPos.y + thePlayer.transform.position.y);
        Vector2 angleVector = new Vector2(collision.gameObject.transform.position.x - localPos.x, collision.gameObject.transform.position.y - localPos.y);
        blockEffectCooldown = true;
        Invoke(nameof(EnableBlockEffect), 0.3f);
        //// average out contact point locations
        //float xSum = 0;
        //float ySum = 0;
        //int points = collision.contactCount;
        ////ContactPoint2D[] contactArray = new ContactPoint2D[points];
        //for (int i=0; i<points; i++)
        //{
        //    xSum += collision.GetContact(i).point.x;
        //    ySum += collision.GetContact(i).point.y;
        //    //contactArray[i] = collision.GetContact(i); // fill contactArray with ContactPoint2Ds. Feels redundant but idk how to use collision.GetContacts
        //}
        //float xAv = xSum / points;
        //float yAv = ySum / points;

        //Vector2 angleVector = new Vector2(collision.gameObject.transform.position.x - xAv, collision.gameObject.transform.position.y - yAv);
        float angleDeg = 180/Mathf.PI * Mathf.Atan(angleVector.y / angleVector.x);

        if (!thePlayer.faceRight)
            angleDeg = 180 + angleDeg;

        Instantiate(blockWaveEffect, localPos, Quaternion.Euler(new Vector3(0, 0/*thePlayer.faceRight? 0:180*/, angleDeg)));
        //Instantiate(blockParticles, localPos, Quaternion.identity);
    }

    void CreateBlockEffects(Collider2D collision) // interaction with triggers
    {
        //Vector2 closest = collision.ClosestPoint(this.gameObject.transform.position); // gets closest point between collider and blocker
        blockEffectCooldown = true;
        Invoke(nameof(EnableBlockEffect), 0.3f);

        Vector2 localPos = new Vector2((thePlayer.faceRight ? 1:-1) * fixedPos.x + thePlayer.transform.position.x, fixedPos.y + thePlayer.transform.position.y);
        // find vector angle from closest to projectile position
        Vector2 angleVector = new Vector2(collision.gameObject.transform.position.x - localPos.x, collision.gameObject.transform.position.y - localPos.y);
        // turn vector into angle
        float angleDeg = 180 / Mathf.PI * Mathf.Atan(angleVector.y / angleVector.x);
        // It's 1:00 am. If this works, I'm leaving it
        if (!thePlayer.faceRight)
            angleDeg = 180 + angleDeg;

        Instantiate(blockWaveEffect, localPos, Quaternion.Euler(new Vector3(0, 0/*thePlayer.faceRight? 0:180*/, angleDeg)));
        //Instantiate(blockParticles, localPos, Quaternion.identity);
    }

    void EnableBlockEffect()
    {
        blockEffectCooldown = false;
    }

}// Blocker
