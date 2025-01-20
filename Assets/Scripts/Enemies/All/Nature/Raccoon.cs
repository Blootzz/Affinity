using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : Enemy
{
    public float raccoonHealth = 40; // max health
    public float raccoonMass = 1;
    public float raccoonVulnerability = 1;

    private float raccoonGravity = 3;
    private float attackTime = 1.5f;
    private Vector2 jumpForceVec = new Vector2(5f, 10f);
    private bool jumpingBack = false;
    private bool pouncing = false;
    private bool airbourne = false;

    new // Unity prefers this for some reason. This definitively blocks the parent from executing Start(); then I do it manually. Idk


        // Start is called before the first frame update
        void Start()
    {
        uniqueGravity = raccoonGravity;
        health = raccoonHealth;
        mass = raccoonMass;
        vulnerability = raccoonVulnerability;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        

            // This got complicated because the GroundCheck collider will still overlap the ground sometimes for 1 frame after jumping.
            // Thus, "airbourne" will tell us if we have even left the ground yet
            if (jumpingBack)
            {
                if (!airbourne && !isGrounded)
                {
                    airbourne = true;
                } // sets airbourne to true when leaving the ground but does not yet set it to false when reentering

                if (airbourne && isGrounded)
                {
                    jumpingBack = false;
                    airbourne = false;
                    animator.Play(windUp1);
                } // will be called upon landing
            }// if jumping back
            else
            {
                if (pouncing)
                {
                    if (!airbourne && !isGrounded)
                    {
                        airbourne = true;
                        animator.Play(attack1);
                    } // detects when 

                    if (airbourne && isGrounded)
                    {
                        pouncing = false;
                        airbourne = false;
                        animator.Play(standAggrivated);
                    }
                }// if pouncing
            }//not jumping back
    }// Update


    public override void Attack()
    {
        Invoke(nameof(JumpBack), attackTime);
        attacking = true;
    }

    void JumpBack()
    {
        jumpingBack = true;
        // makes x component of jumpForceVec + if to the right of the player
        rb.AddForce(new Vector2(jumpForceVec.x * (rightOfPlayer ? 1 : -1), jumpForceVec.y) / 3, ForceMode2D.Impulse);
        Invoke(nameof(Pounce), 1.5f);
        
    }

    void Pounce()
    {
        pouncing = true;
        rb.AddForce(new Vector2(jumpForceVec.x * (rightOfPlayer ? -1 : 1), jumpForceVec.y), ForceMode2D.Impulse);
        StartCoroutine(SetAttacking(false, 2f)); // amount of time until the raccoon is ready to begin a new attack
        animator.Play(attack1);
    }

}