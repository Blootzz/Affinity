using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSmall : Enemy
{
    [SerializeField]
    float uniqueHealth = 30; // max health
    [SerializeField]
    float uniqueMass = 1;

    public override void Start()
    {
        health = uniqueHealth;
        mass = uniqueMass;
        flipLocked = true;
        aggrivated = true;
        collisionDamage = 20;
        base.Start();
    }// Start

    public override void NearbyLogic() // called every frame if stunned=false and attacking=false
    {
        animator.Play(walkAggrivated);
    }

    public override void AggrivatedLogic() // called every frame if attacking=false, stunned=false, nearby=true
    {
        animator.Play(walkAggrivated);
    }

    public override void Update()
    {
        base.Update();
    }

}
