using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anselm : Player
{
    [HideInInspector] public Shield playerShield; // used in Rope.cs
    float AnselmMoveSpeed = .1f;
    float AnselmJumpHeight = 15f;
    float AnselmVerticalNudgeHeight = 3f;
    float AnselmMaxHealth = 100;
    float AnselmMaxStamina = 100;
    float AnselmMaxBarrierHealth = 100;
    float AnselmGravity = 4;
    float AnselmMass = 1;
    Vector2 AnselmLedgeGrabOffset = new Vector2(0.3f, 0.85f);// value subtracted from ledge position to properly allign

    // Attack Data
    float quickThrowDamage = 20;
    float quickThrowStunTime = 0.2f;
    float quickThrowKnockbackMultiplier = 1;
    Vector2 quickThrowKnockback = new Vector2(1, 0);
    float straightAttackDamage = 20;
    float straightAttackStunTime = 0.2f;
    float straightAttackKnockbackMultiplier = 1;
    Vector2 straightAttackKnockback = new Vector2(1, 0);
    float downAttackDamage = 20;
    float downAttackStunTime = 0.2f;
    float downAttackKnockbackMultiplier = 1;
    Vector2 downAttackKnockback = new Vector2(1, 0);
    float upAttackDamage = 20;
    float upAttackStunTime = 0.2f;
    float upAttackKnockbackMultiplier = 1;
    Vector2 upAttackKnockback = new Vector2(1, 2);


    
    /*
    // jab1
    float jab1Damage = 5f;
    float jab1StunTime = .5f;
    float jab1KnockbackMultiplier = -1;
    Vector2 jab1Angle = new Vector2(1, -1);

    //jab 2
    float jab2Damage = 10f;
    float jab2StunTime = .6f;
    float jab2KnockbackMultiplier = 1;
    Vector2 jab2Angle = new Vector2(0, 1);
    
    // jab3
    float jab3Damage = 15f;
    float jab3StunTime = 1f;
    float jab3KnockbackMultiplier = 5;
    Vector2 jab3Angle = new Vector2(1, 5);
    */ // 3 hit jabs

    // overhead
    float overheadDamage = 30f;
    float overheadStunTime = 1.5f;
    float overheadKnockbackMultiplier = 2;
    Vector2 overheadAngle = new Vector2(3, 1);

    new


        // Start is called before the first frame update
        void Start()
    {
        playerShield = FindObjectOfType<Shield>(); //finds a shield
        InitializeData();
        base.Start();
    }// Start

    new

        // Update is called once per frame
        void Update()
    {
        if(!controlsDisabled)
            ExecuteControls();     // Checks input and executes player controls accordingly
        base.Update();  // Player.Update()
    }// Update

    void ExecuteControls()
    {
        if (!onLedge) // don't do the following when in a special state like onLedge
        {
            //===================================================== Basic Attacks ========================================================
            if (Input.GetKeyDown(GameMaster.GM.controlManager.attackKey) && !playerShield.shieldDeployed)
            {
                if (isGrounded) //-------------- Grounded Attacks ------------------------
                {
                    GroundedAndAerialAttacks();
                    GroundedAttacks(); // exclusive ground attacks
                                       // If GroundedAttacks() calls an animation, it will overwrite animation played by GroundedAndAerialAttacks()
                } // grounded

                else // ------------------------ Aerial Attacks ------------------------
                {
                    GroundedAndAerialAttacks();
                    AerialAttacks(); // exclusive aerial attacks
                                     // If AerialAttacks() calls an animation, it will overwrite animation played by GroundedAndAerialAttacks()
                }// not grounded
            }// basic attacks
            else //================================================ Shield Throw =======================================
            {
                // Can be input regardless of direction
                if (CheckCanThrow()) // will work regardless of inLag
                {

                    if (!playerShield.shieldDeployed && stamina > 0)
                    {
                        animator.Play(Throw); //_Yeet() is called on the first frame of this animation
                    }
                    else // shield is deployed
                        playerShield.EngageShieldAbility();
                }// throwKey
            }// Shield Throw
        }// not in special state like onLedge
    }// Controls

    public override void WhileBlocking()
    {
        if (!playerShield.shieldDeployed) // checks if shield is available before blocking
            base.WhileBlocking();
    }


    //==================================== Initialization w/ Player class ====================================
    void InitializeData()
    {
        InitializeMovementData();
        InitializeHealthData();
    }

    void InitializeHealthData()
    {
        maxHealth = AnselmMaxHealth;
        maxBarrierHealth = AnselmMaxBarrierHealth;
        maxStamina = AnselmMaxStamina;
    }

    void InitializeMovementData()
    {
        moveSpeed = AnselmMoveSpeed;
        jumpHeight = AnselmJumpHeight;
        gravity = AnselmGravity;
        mass = AnselmMass;
        ledgeGrabOffset = AnselmLedgeGrabOffset;
        verticalNudgeHeight = AnselmVerticalNudgeHeight;
    }

    //==================================== Attacks ====================================
    void GroundedAttacks() // attacks that can be done EXCLUSIVELY on the ground
    {
        if (vert == 0) //-------------------------------- Vertically Neutral Attacks --------------------------
        {

        }
        else //------------------------------------------ Up & Down Attacks -------------------------------------
        {
            if (vert > 0) // upwards attacks
            {

            }
            else // low attacks
            {

            }
        }// Up & Down Attacks
    }
    void AerialAttacks() // attacks that can be done EXCLUSIVELY in the air
    {
        if (vert == 0) //-------------------------------- Vertically Neutral Attacks --------------------------
        {

        }
        else //------------------------------------------ Up & Down Attacks -------------------------------------
        {
            if (vert > 0) // upwards attacks
            {

            }
            else // low attacks
            {
                SetUpAttack(downAttackDamage, downAttackStunTime, downAttackKnockbackMultiplier, downAttackKnockback);
                animator.Play(DownAttack);
            }
        }// Up & Down Attacks
    }// AerialAttacks()
    void GroundedAndAerialAttacks() // attacks that can be done in the air or on the ground
    {
        if (vert == 0) //--------------------------------- Vertically Neutral Attacks ---------------------------------
        {
            if (GetComponent<SHORYUKEN>().CheckShoryu()) // checks if last directional movements are the shoryuken inputs
                GetComponent<SHORYUKEN>().DoShoryu();
            else
            {
                SetUpAttack(straightAttackDamage, straightAttackStunTime, straightAttackKnockbackMultiplier, straightAttackKnockback);
                animator.Play(StraightAttack);
            }
        } // Vertically neutral attacks
        else //--------------------------------- Up & Down Attacks ---------------------------------
        {
            if (vert > 0) // high attacks
            {
                SetUpAttack(upAttackDamage, upAttackStunTime, upAttackKnockbackMultiplier, upAttackKnockback);
                animator.Play(UpAttack);
            }
            else // low attacks         vert < 0 (not equal to)
            {

            }
        }// Up or down attacks
    }

    /*
    void JabA()
    {
        if (jabNum == 0 && !inLag)
        {
            Jab1Start();
            return;
        }
        if (jabNum == 1 && attackConnected && lightCancellable)
        {
            Jab2Start();
            return;
        }
        if (jabNum == 2 && attackConnected && lightCancellable)
        {
            Jab3Start();
            return;
        }

    }

    void Jab1Start()
    {
        SetUpAttack(jab1Damage, jab1StunTime, jab1KnockbackMultiplier, jab1Angle);
        //animator.Play(Jab1Animation);
        jabNum = 1;
    }
    void Jab2Start()
    {
        SetUpAttack(jab2Damage, jab2StunTime, jab2KnockbackMultiplier, jab2Angle);
        //animator.Play(Jab2Animation);
        jabNum = 2;
    }
    void Jab3Start()
    {
        SetUpAttack(jab3Damage, jab3StunTime, jab3KnockbackMultiplier, jab3Angle);
        //animator.Play(Jab3Animation);
        jabNum = 3;
    }
    */ // Old 3-jab code

    void Overhead()
    {
        SetUpAttack(overheadDamage, overheadStunTime, overheadKnockbackMultiplier, overheadAngle);
        //animator.Play(OverheadAnimation);
    }

    // ==================== AnimationEvents ====================
    //void _Check_Heavy_Throw()
    //{
    //    if (Input.GetKey(throwKey))
    //    {
    //        animator.Play(ThrowHeavy);
    //    }
    //}

    void _YEET()
    {
        if (stamina > 0)
        {
            attacking = true;
            playerShield.Yeet(speed.y, 1.0f * (float)(staminaDepleted ? 0.75:1));
            ToggleAorUAnimations();
            UseStamina(throwStamina);
            if (isGrounded)
                CreateThrowDustEmitter();
            else
                VerticalNudge();
        }
    }
    void _YEET_HEAVY()
    {
        if (stamina > 0)
        {
            attacking = true;
            playerShield.Yeet(speed.y, 1.2f);
            ToggleAorUAnimations();
            if (isGrounded)
                CreateThrowDustEmitter();
        }
    }

    void test()
    {
        print("this is a test");
    }

}
