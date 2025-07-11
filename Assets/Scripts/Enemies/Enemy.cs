using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Player thePlayer;

    public float health = 100;
    [HideInInspector] public float vulnerability = 1; // damage taken mulitplier
    [HideInInspector] public float mass = 1;
    [HideInInspector] public float uniqueGravity = 1;
    [HideInInspector] public bool flipLocked = false;
    [HideInInspector] public float detectionRadius = 5; // range at which enemy will see player
    [HideInInspector] public float collisionDamage = 10;
    bool dead = false;
    [SerializeField]
    bool nearby = false;
    //[HideInInspector]
    public bool aggrivated = false;
    [HideInInspector] public bool stunned = false; // indicator of stunned state
    /*[HideInInspector]*/ public bool rightOfPlayer = true;
    /*[HideInInspector]*/ public bool faceRight = true;
    [HideInInspector] public bool attacking = false;
    /*[HideInInspector]*/ public bool isGrounded = false;

    [HideInInspector] public int idle;
    [HideInInspector] public int walkPeaceful;
    [HideInInspector] public int walkAggrivated;
    [HideInInspector] public int stun;
    [HideInInspector] public int standAggrivated;
    [HideInInspector] public int windUp1;
    [HideInInspector] public int attack1;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemies");
        rb = GetComponent<Rigidbody2D>();
        rb.mass = mass; // adjust the mass
        rb.gravityScale = uniqueGravity; // adjust the gravity
        thePlayer = GameMaster.GM.thePlayer;
        animator = GetComponent<Animator>();

        // FILE NAMES MUST BE EXACT! Ex: in order to play the idle animation for a Dummy, the animation MUST be called "DummyIdle"
        InstantiateAnimations();
    }

    // Update is called once per frame
    public virtual void Update() // "virtual" means it can be overridden
    {
        // determine if player is on left or right of enemy
        if (gameObject.transform.position.x > thePlayer.transform.position.x)
        {
            rightOfPlayer = true;
        }
        else
        {
            rightOfPlayer = false;
        }

        if ((Mathf.Abs(transform.position.x - thePlayer.transform.position.x) < detectionRadius) &&
            Mathf.Abs(transform.position.y - thePlayer.transform.position.y+thePlayer.offsetY) < detectionRadius)
            nearby = true;
        else
            nearby = false;

        if (!attacking)
        {
            if (!stunned)
            {
                if (nearby)
                {
                    // ====================== STANDARD BEHAVIOR ======================

                    // Will keep the enemy facing player
                    if (faceRight == rightOfPlayer && !flipLocked)
                    {
                        Flip();
                    }// if not facing correct way

                    if (!aggrivated)
                    {
                        if (CheckLineOfSight()) // CheckLineOfSight returns true if a raycast can be drawn to the player
                        {
                            BecomeAggrivated();
                        }
                    }// if not aggrivated
                    else
                    {
                        AggrivatedLogic();
                    }// aggrivated

                } // nearby

                if (!attacking) // Necessary to check again because attacking may get set to true earlier
                    NearbyLogic();

            }// not stunned
        }// not attacking

    }// Update


    void BecomeAggrivated()
    {
        aggrivated = true;
        //animator.Play(standAggrivated);
    }// BecomeAggrivated

    public virtual void NearbyLogic()// called every frame if not attacking and not stunned
    {
        animator.Play(idle);
    }

    public virtual void AggrivatedLogic()
    {
        //attacking = true;
    }
    
    public virtual void Attack()
    {
        //animator.Play(attack1);
        //print("Enemy Attack");
    }// Attack

    private void InstantiateAnimations() // Relies on naming convention
    {
        idle = Animator.StringToHash(name + "Idle");
        walkPeaceful = Animator.StringToHash(name + "WalkPeaceful");
        walkAggrivated = Animator.StringToHash(name + "WalkAggrivated");
        standAggrivated = Animator.StringToHash(name + "StandAggrivated");
        stun = Animator.StringToHash(name + "Stunned");
        windUp1 = Animator.StringToHash(name + "WindUp1");
        attack1 = Animator.StringToHash(name + "Attack1");
    }

    public bool CheckLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y+thePlayer.offsetY) - transform.position);
        return hit.collider == thePlayer.gameObject.GetComponent<Collider2D>(); // returns true or false
    }

    public virtual void TakeDamage(float damage, float stunTime, float knockbackMultiplier, Vector2 launchDirection) // Oh yeah, it's Smashing time
    {
        CancelInvoke();
        rb.gravityScale = 1;
        attacking = false;

        // Turns launch direction into unit vector. Reverses x direction if player is to enemy's left side
        launchDirection = new Vector2(
            x: (rightOfPlayer ? 1 : -1) * (launchDirection.x / launchDirection.magnitude),
            y: launchDirection.y / launchDirection.magnitude
            );

        health -= damage * vulnerability;
        if (!dead && health <= 0) // GET HIM OUTTA HERE
        {
            Die();
            dead = true;
            return;
        }

        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0,0); // prevents knockback from accumulating
        GetComponent<Rigidbody2D>().AddForce(launchDirection * knockbackMultiplier, ForceMode2D.Impulse);

        // ==================Hit effects===============
        // play sound of getting hit
        // material-specific particle effect (sparks, feathers, energy)

        if (stunTime > 0) //For some reason, if Stun(0) is called, Enemy gets permanently stunned (also, it's more efficient)
        {
            Stun(stunTime);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void Stun(float stunTime)
    {
        CancelInvoke(nameof(Unstun)); // Cancels unstun if it was called earlier
        attacking = false;
        stunned = true;
        animator.Play(stun);
        Invoke(nameof(Unstun), stunTime); //After <stunTime> seconds, call the method Unstun
    }

    private void Unstun()
    {
        stunned = false;
        animator.Play(idle);
        rb.gravityScale = uniqueGravity;
    }

    public void Flip()
    {
        transform.Rotate(Vector3.up * 180);
        faceRight = !faceRight;
    }

    public IEnumerator SetAttacking(bool tf, float delay) // exists so I can use Invoke
    {
        yield return new WaitForSeconds(delay);
        attacking = tf;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Does damage to player if they collide (MOST BASIC ENEMY ATTACK)
    {
        // Player may hold out shield (edge collider, no trigger)
        // Shield edge collider is a child of Player
        //print("Enemy Colliding with: "+collision.         gameObject.name); // returns parent name
        //print("Enemy Colliding with: "+collision.collider.gameObject.name); // returns child name
        // what the fuck

        if (!stunned && collision.collider.gameObject.CompareTag("Player"))
        {
            thePlayer.TakeHit(collisionDamage, new Vector2(7, 10), rightOfPlayer);
        }
    }

    public void Land()
    {
        animator.Play(idle); // cancels aerial animations
    }

    void _AttackingFalse()
    {
        attacking = false;
        animator.Play(idle);
    }    

}
