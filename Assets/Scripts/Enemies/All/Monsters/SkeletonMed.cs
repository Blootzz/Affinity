using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMed : Enemy
{
    [SerializeField]
    float uniqueHealth = 80; // max health
    [SerializeField]
    float uniqueMass = 1;
    [SerializeField]
    float uniqueDetectionRadius = 10;
    [SerializeField]
    GameObject bone;
    
    public override void Start()
    {
        health = uniqueHealth;
        mass = uniqueMass;
        detectionRadius = uniqueDetectionRadius;
        base.Start();
    }// Start

    void WindUp()
    {
        animator.Play(windUp1);
    }

    public override void AggrivatedLogic()
    {
        if (isGrounded)
        {
            WindUp();
            attacking = true;
        }
    }

    void _Attack()
    {
        animator.Play(attack1);
    }

    void _BoneThrow()
    {
        float offset = 0.6f * Mathf.Round(Random.value);
        GameObject newBone = Instantiate(bone, new Vector2(transform.position.x + (faceRight ? 1f : -1f), transform.position.y - offset), Quaternion.identity);
        newBone.GetComponent<Projectile>().moveRight = faceRight;
        Destroy(newBone, 2f);
    }

}
