using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkBlind : MonoBehaviour
{
    Enemy thisEnemy;
    public float walkSpeed; // establish in inspector
    int count;

    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<Enemy>();
        thisEnemy.flipLocked = true;
        //ISSUE: attack gets turned true and freezes the enemy
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if available to move
        if(!thisEnemy.attacking && !thisEnemy.stunned && thisEnemy.isGrounded)
        {
            // walk forward
            thisEnemy.rb.MovePosition(transform.position + new Vector3(Time.timeScale * walkSpeed * (thisEnemy.faceRight ? 1:-1), 0));
        }// not attacking or stunned
        
    }

    public void ActAtLedgeOrWall() // called by EdgeCheck.cs
    {
        // if the enemy is already clipping into the wall, this may get triggered by whatever is detecting the wall
        thisEnemy.Flip();
    }
}
