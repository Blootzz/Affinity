using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    // Ctrl + M + H to collapse code
    // Ctrl + M + U to undo collapse code

    // Object References
    [HideInInspector]
    public Animator animator;
    AnimatorStateInfo animatorState; // use animator.shortNameHash to see what is playing
    Rigidbody2D rb;
    TimeController tController;
    GameObject camController;
    Animator camControllerAnimator;
    SpriteRenderer sr;
    GameObject tileMap;
    Hitbox1Script hitbox1;
    [HideInInspector]
    //public WallJumpCheck wallJumpCheck;
    //Stopwatch stopWatch;

    //// References to prefabs so player can drag in all of these items when placed in an empty room
    //public Shield loadOnlyShield;
    //public Canvas loadOnlyCanvas;
    //public Camera loadOnlyMainCam;

    // Movement
    Vector3 movement = new Vector3(0, 0, 0);
    [HideInInspector]
    public float offsetY = 0.8f;
    float coyoteTime = 0.15f;
    float coyoteTimeCounter;
    float jumpBufferTime = 0.1f;
    float jumpBufferCounter;
    [HideInInspector]
    public Vector3 speed = new Vector3(0, 0, 0);
    [HideInInspector]
    public Vector2 ledgeGrabOffset;// Changed in Anselm.cs
    [HideInInspector]
    public float hor = 0; // value of horizontal input [-1;1]
    [HideInInspector]
    public float vert = 0; // value of vertical input [-1;1]
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public float jumpHeight;
    [HideInInspector]
    public float maxBarrierHealth;
    [HideInInspector]
    public float gravity;
    [HideInInspector]
    public float mass;
    float playerStunTime = 0.2f;
    bool isStunned = false;
    [HideInInspector]
    public bool faceRight = true;
    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public bool controlsDisabled = false;
    //[HideInInspector]
    public bool isFalling = false;
    [HideInInspector]
    public bool ledgeGrabActive = false;
    [HideInInspector]
    public bool onLedge = false;
    bool ledgeGrabCooldown = false;
    [HideInInspector]
    public float verticalNudgeHeight;
    public bool isIgnoringHorInput = false; // used to restrict wall jump to start with an arc
    public bool isBusy = false; // used in Rope.cs to tell when another class is borrowing control of player

    // Combat
    // Attack variables
    [HideInInspector]
    public bool inLag = false;
    //[HideInInspector]
    public bool attacking = false;
    [HideInInspector]
    public bool attackConnected = false;
    [HideInInspector]
    public bool lightCancellable = false;
    [HideInInspector]
    public bool throwing = false; // Changed in Throw animation to ensure player still has full control even though animation is playing
    public float attackDamage;
    [HideInInspector]
    public Vector2 attackKnockbackAngle;
    [HideInInspector]
    public float attackKnockbackMultiplier;
    [HideInInspector]
    public float attackStunTime;
    [HideInInspector]
    public bool blocking = false; // used in Anselm.cs to check if throw can be executed
    [HideInInspector]
    public bool parryWindowOpen = false;

    // Jabs
    [HideInInspector]
    public int jabNum = 0;

    // ============================health & stamina
    readonly float invincibilityTime = 2f;
    readonly float maxStaminaRegenRate = 0.5f;
    readonly float depletedStaminaRegenRate = 0.25f;
    readonly float guardStaminaRate = -0.25f;
    readonly float parryStamina = 15f;
    bool invincible = false;
    public float health;
    float barrierHealth;
    //bool barrierBroken = false;
    [HideInInspector]
    public float maxHealth;
    public float stamina;
    [HideInInspector]
    public float maxStamina; // set by anselm
    public float staminaRegenRate;
    float tempOriginalStaminaRate; // used to remember stamina regen rate while blocking
    [HideInInspector]
    public bool staminaDepleted = false;
    [HideInInspector]
    public bool dead = false;

    // stamina costs
    float jumpStamina = 25f;
    [HideInInspector]
    public float throwStamina = 30f;

    // Animation Hashes
    // Armed
    [SerializeField]
    public int Idle;
    int Pant;
    int Run;
    int Stunned;
    [SerializeField]
    public int Falling;
    int FallingExt;
    int Reach;
    int Hanging;
    int ClimbLedge;
    int LedgeJump;
    int WallSlide;
    int Crouch;
    // Unarmed
    int IdleUnarmed;
    int PantUnarmed;
    int RunUnarmed;
    int StunnedUnarmed;
    int FallingUnarmed;
    int FallingExtUnarmed;
    int ReachUnarmed;
    int HangingUnarmed;
    int ClimbLedgeUnarmed;
    int LedgeJumpUnarmed;
    int WallSlideUnarmed;
    int CrouchUnarmed;
    // Combat
    int Block;
    int BlockUp;
    int Parry;
    int ParryUp;
    [HideInInspector]
    public int QuickThrow;
    [HideInInspector]
    public int StraightAttack;
    [HideInInspector]
    public int UpAttack;
    [HideInInspector]
    public int DownAttack;
    int Jab1Animation;
    int Jab2Animation;
    int Jab3Animation;
    int OverheadAnimation;
    [HideInInspector]
    public int Throw;
    int ThrowHeavy;

    // Special Abilities
    // Zipline (used in Rope.cs)
    [HideInInspector] public int ZiplineForward;
    [HideInInspector] public int ZiplineBackward;
    //[HideInInspector] public int ZiplineIdle;
    [HideInInspector] public int ZipAttackForward;
    [HideInInspector] public int ZipAttackBackward;
    [HideInInspector] public int ZiplineApproach;

    // Armed or Unarmed version of animation will keep track of which of the two animations to play (Idle means armed Idle)
    int AorUIdle;
    int AorUPant;
    int AorURun;
    int AorUStunned;
    [HideInInspector] public int AorUFalling;
    int AorUFallingExt;
    [HideInInspector]
    public int AorUReach;
    [HideInInspector]
    public int AorUHanging;
    int AorUClimbLedge;
    int AorULedgeJump;
    [HideInInspector]
    public int AorUWallSlide;
    int AorUCrouch;
    // changes idle to pant depending on how much health
    int DynamicIdle;

    // UI Stuff
    ////[HideInInspector]
    //public HealthBar healthBar;
    ////[HideInInspector]
    //public StaminaBar staminaBar;

    // Particles
    bool landingDustEnabled = true;
    [HideInInspector]
    public ParticleSystem landingDust;
    [HideInInspector]
    public ParticleSystem jumpDust;
    [HideInInspector]
    public ParticleSystem throwDust;

    // Start is called before the first frame update
    public void Start()
    {
        tileMap = GameObject.FindGameObjectWithTag("Ground");
        tController = new TimeController(); // TimeController needs to use Monobehaviour.FixedUpdate but it can't inherit Monobehaviour. We'll do it in Player
        camController = GameObject.FindGameObjectWithTag("CameraController");
        camControllerAnimator = camController.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hitbox1 = GetComponentInChildren<Hitbox1Script>();
        rb.mass = mass;
        rb.gravityScale = gravity;
        SetHealth(maxHealth);
        SetStamina(maxStamina);
        barrierHealth = maxBarrierHealth;
        staminaRegenRate = maxStaminaRegenRate;
        tempOriginalStaminaRate = staminaRegenRate;
        InstantiateAnimations();
        animator.Play(DynamicIdle);
        //stopWatch = new Stopwatch();
        rb.isKinematic = false;
        tController.UpdateFixedDeltaTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Stamina regen
        AddStamina(staminaRegenRate);
        //staminaBar.SetStaminaSlider(stamina);

        if (!controlsDisabled)
        {
            if (!rb.isKinematic && !blocking) // will not be able to move while blocking
            {
                //if (!attacking) // prevents changing direction while attacking (slide forward)
                    movement = new Vector3(hor, 0, 0);
                speed = rb.linearVelocity;
                transform.position += movement * moveSpeed * Time.timeScale;
            }// dynamically controlled
        }// !controlsDisabled

        if (tController.controllerActive) // only call when the time controller is doing stuff
        {
            tController.FixedUpdate(); // need an instance of tController so we can run fixedUpdate without monobehaviour

            if (!tController.controllerActive) // if controllerActive became false after FixedUpdate, transition back to CM vcam1
            {
                camControllerAnimator.SetBool("TimeController_Active", false);
            }
        }

    }// fixed Update

    public void Update()
    {
        // test
        if (Input.GetKeyDown(GameMaster.GM.controlManager.testKey))
            Test();

        if (!isIgnoringHorInput)
        {
            hor = Input.GetAxisRaw("Horizontal");
            vert = Input.GetAxisRaw("Vertical");
        }
        else
        {
            hor = 0;
            vert = 0;
        }

        if (!controlsDisabled && !isBusy)
        {
            if (!rb.isKinematic) // affected by physics
            {
                // Grounded or Aerial
                // Movment input
                if (!attacking && !throwing)
                {
                    LeftOrRight(); // chooses whether or not to Flip()

                    if (Input.GetKeyDown(GameMaster.GM.controlManager.spellMenuKey))
                        OpenSpellMenu();
                }// not attacking

                // ====================================== grounded controls & Animations ==================================
                if (isGrounded)
                {
                    if (!attacking)
                    {
                        CheckBlocking(); // sets blocking to true or false, adjusts stamina rates
                        if (blocking)
                            WhileBlocking(); // determines directional blocking, parrying

                        if (!blocking)
                        {
                            // Jump
                            if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey) || jumpBufferCounter > 0)
                            {
                                // changing GetKeyDown to GetKey causes minecraft jumping (instantaneous upon hitting ground)
                                // if jumpBufferCounter is positive, a jump will be executed as soon as isGrounded is true
                                Jump();
                                jumpBufferCounter = 0;
                            } // using this frame to jump
                            else
                            {
                                if (!throwing) // prevents run or idle animations from interrupting shield throw
                                {
                                    // Running or Idle
                                    if (hor != 0 && animatorState.shortNameHash != AorURun)
                                    {
                                        animator.Play(AorURun);
                                    }
                                    if (hor == 0)
                                    {
                                        // look up
                                        if (Input.GetKey(GameMaster.GM.controlManager.upKey))
                                {

                                }

                                        // crouch
                                        if (Input.GetKey(GameMaster.GM.controlManager.downKey))
                                            animator.Play(AorUCrouch);
                                        else
                                            animator.Play(DynamicIdle);
                                    }

                                }// not throwing
                            }// not jumping on this frame
                        }// not blocking
                    }// not busy with an attack

                    coyoteTimeCounter = coyoteTime;

                }// grounded
                else// aerial controls & animations
                {
                    coyoteTimeCounter -= Time.deltaTime;
                    jumpBufferCounter -= Time.deltaTime; // window to buffer a jump decreases

                    if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey) && coyoteTimeCounter > 0)
                    {
                        Jump();
                    }// Coyote Time jump

                    if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey))
                    {
                        // reset jumpBufferCounter to its maximum
                        jumpBufferCounter = jumpBufferTime;
                    }

                    if (!attacking)
                    {
                        // Either reaching or plain airbourne animations
                        if (Input.GetKey(GameMaster.GM.controlManager.ledgeGrabKey) && !ledgeGrabCooldown)
                        {
                            animator.Play(AorUReach);
                            //ledgeGrabActive = true;
                            //wallJumpCheck.gameObject.GetComponent<BoxCollider2D>().enabled = false;

                        }
                        else if (!throwing) // tried animatorState.shortNameHash ==
                        {
                            ledgeGrabActive = false;
                            //if (isFalling)
                            //    wallJumpCheck.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        // Check if apex has been reached
                        if (!isFalling && rb.linearVelocity.y < 0) // Begin falling
                        {
                            isFalling = true;
                            //if (!ledgeGrabActive) // prevent ledge grab and wall slide from competing with each other, giving priority to ledge grab to avoid going into kinematic mode
                            //    wallJumpCheck.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        }

                    }// !attacking

                    // Cancel Jump
                    if (Input.GetKeyUp(GameMaster.GM.controlManager.jumpKey) && rb.linearVelocity.y > 0)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                        coyoteTimeCounter = 0;
                    }

                }// aerial
            }// not in kinematic mode
            else
            {
                // Kinematic Controls

                // controls for when hanging on ledge
                if (onLedge)
                {
                    // Release Ledge
                    if (Input.GetKeyDown(GameMaster.GM.controlManager.ledgeGrabKey))
                    {
                        animator.Play(AorUFalling);
                        rb.linearVelocity = new Vector2((faceRight ? -1 : 1) * moveSpeed, 0);
                        onLedge = false;
                        ledgeGrabCooldown = true;
                        Invoke("DisableLedgeGrabCooldown", 0.2f);// Prevents instantaneously regrabbing. Unfortunately, disabling LedgeGrabCheck's collider triggers OnExit2D
                        rb.bodyType = RigidbodyType2D.Dynamic; // NO ANIMATION ==> NEVER CALLS Set_isKinematic_False() IN AN ANIMATION
                    }// release the ledge

                    // Climb Up
                    if ((faceRight && Input.GetKeyDown(GameMaster.GM.controlManager.rightKey)) || (!faceRight && Input.GetKeyDown(GameMaster.GM.controlManager.leftKey)))
                    {
                        animator.Play(AorUClimbLedge);
                        onLedge = false;
                        // Animation calls Set_isKinematic_False() at the end of kinematic movement
                    }// Climbs up while facing either way

                    // Ledge Jump
                    if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey) && stamina > 0)
                    {
                        animator.Play(AorULedgeJump);
                        onLedge = false;
                        // Animation calls Set_isKinematic_False() at the end of kinematic movement
                    }// Jumps from ledge

                }// onLedge

                // wall slide logic controlled in WallJumpCheck.cs

            }// kinematic mode
        }// !controlsDisabled
        else 
        {
            // controlsDisabled = true in cutscene
            if (!isStunned && !isBusy)
            {
                if (isGrounded)
                    animator.Play(DynamicIdle);
                else
                {
                    animator.Play(AorUFalling);
                }
            }// not stunned
        }// controls are disabled
    }// Update

    // ============================================ ANIMATIONS ====================================
    private void InstantiateAnimations() // Relies on naming convention
    {
        // Regular
        Idle = Animator.StringToHash(name + "Idle");
        //Walk = Animator.StringToHash(name + "Walk");
        Pant = Animator.StringToHash(name + "Pant");
        Run = Animator.StringToHash(name + "Run");
        Stunned = Animator.StringToHash(name + "Stunned");
        Falling = Animator.StringToHash(name + "Falling");
        FallingExt = Animator.StringToHash(name + "FallingExt");
        Reach = Animator.StringToHash(name + "Reach");
        Hanging = Animator.StringToHash(name + "Hanging");
        ClimbLedge = Animator.StringToHash(name + "ClimbLedge");
        LedgeJump = Animator.StringToHash(name + "LedgeJump");
        WallSlide = Animator.StringToHash(name + "WallSlide");
        Crouch = Animator.StringToHash(name + "Crouch");

        // Unarmed
        IdleUnarmed = Animator.StringToHash(name + "IdleUnarmed");
        PantUnarmed = Animator.StringToHash(name + "PantUnarmed");
        RunUnarmed = Animator.StringToHash(name + "RunUnarmed");
        StunnedUnarmed = Animator.StringToHash(name + "StunnedUnarmed");
        FallingUnarmed = Animator.StringToHash(name + "FallingUnarmed");
        FallingExtUnarmed = Animator.StringToHash(name + "FallingExtUnarmed");
        ReachUnarmed = Animator.StringToHash(name + "ReachUnarmed");
        HangingUnarmed = Animator.StringToHash(name + "HangingUnarmed");
        ClimbLedgeUnarmed = Animator.StringToHash(name + "ClimbLedgeUnarmed");
        LedgeJumpUnarmed = Animator.StringToHash(name + "LedgeJumpUnarmed");
        WallSlideUnarmed = Animator.StringToHash(name + "WallSlideUnarmed");
        CrouchUnarmed = Animator.StringToHash(name + "CrouchUnarmed");

        // attacks
        Block = Animator.StringToHash(name + "Block");
        BlockUp = Animator.StringToHash(name + "BlockUp");
        Parry = Animator.StringToHash(name + "Parry");
        ParryUp = Animator.StringToHash(name + "ParryUp");
        QuickThrow = Animator.StringToHash(name + "QuickThrow");
        StraightAttack = Animator.StringToHash(name + "StraightAttack");
        UpAttack = Animator.StringToHash(name + "UpAttack");
        DownAttack = Animator.StringToHash(name + "DownAttack");
        Jab1Animation = Animator.StringToHash(name + "Jab1");
        Jab2Animation = Animator.StringToHash(name + "Jab2");
        Jab3Animation = Animator.StringToHash(name + "Jab3");
        OverheadAnimation = Animator.StringToHash(name + "Overhead");
        Throw = Animator.StringToHash(name + "Throw");
        ThrowHeavy = Animator.StringToHash(name + "ThrowHeavy");

        // Special Abilities
        ZiplineForward = Animator.StringToHash(name + "ZiplineForward");
        ZiplineBackward = Animator.StringToHash(name + "ZiplineBackward");
        //ZiplineIdle = Animator.StringToHash("ZiplineForward"); 
        ZipAttackForward = Animator.StringToHash("ZipAttackForward");
        ZipAttackBackward = Animator.StringToHash("ZipAttackBackward");
        ZiplineApproach = Animator.StringToHash("ZiplineApproach");

        SetChangingAnimations();
    }

    void SetChangingAnimations()
    {
        AorUIdle = Idle;
        AorUPant = Pant;
        AorURun = Run;
        AorUStunned = Stunned;
        AorUFalling = Falling;
        AorUFallingExt = FallingExt;
        AorUReach = Reach;
        AorUHanging = Hanging;
        AorUClimbLedge = ClimbLedge;
        AorULedgeJump = LedgeJump;
        AorUWallSlide = WallSlide;
        AorUCrouch = Crouch;

        DynamicIdle = Idle;
    }

    public void ToggleAorUAnimations()
    {
        if (AorURun != Run) // if variable animations are set to Unarmed
        {
            AorUIdle = Idle;
            AorUPant = Pant;
            AorURun = Run;
            AorUStunned = Stunned;
            AorUFalling = Falling;
            AorUFallingExt = FallingExt;
            AorUReach = Reach;
            AorUHanging = Hanging;
            AorUClimbLedge = ClimbLedge;
            AorULedgeJump = LedgeJump;
            AorUCrouch = Crouch;
        }
        else
        {
            // set all changing animations to Unarmed
            AorUIdle = IdleUnarmed;
            AorUPant = PantUnarmed;
            AorURun = RunUnarmed;
            AorUStunned = StunnedUnarmed;
            AorUFalling = FallingUnarmed;
            AorUFallingExt = FallingExtUnarmed;
            AorUReach = ReachUnarmed;
            AorUHanging = HangingUnarmed;
            AorUClimbLedge = ClimbLedgeUnarmed;
            AorULedgeJump = LedgeJumpUnarmed;
            AorUCrouch = CrouchUnarmed;
        }

        if (DynamicIdle == Pant || DynamicIdle == PantUnarmed)
        {
            DynamicIdle = AorUPant; // Keep panting but with new armed/unarmed condition
        }
        else
        {
            DynamicIdle = AorUIdle; // Keep idling with correct armed/unarmed condition
        }
    }
    
    void Blink()
    {
        if (sr.enabled)
            sr.enabled = false;
        else
            sr.enabled = true;
    }

    // ======================================== STATE =======================================
    void Unstun()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        DisableEndLag(); // Possibly causes controls to be disabled???? No idea how
        controlsDisabled = false;
        isStunned = false;
        // begin blinking
        InvokeRepeating(nameof(Blink), 0f, .1f);
        // resume falling if necessary
        CheckIfFalling();
    }

    public void EnableControls()
    {
        StartCoroutine(nameof(EnableControlsLater)); // necessary to wait 1 frame so dialogue input doesn't also become gameplay input
    }

    IEnumerator EnableControlsLater()
    {
        yield return new WaitForEndOfFrame();
        controlsDisabled = false;
    }

    void CheckIfFalling()
    {
        if (!isGrounded)
            animator.Play(AorUFalling);
    }

    void DisableInvincibility()
    {
        invincible = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        // disable blinking
        CancelInvoke(nameof(Blink));
        sr.enabled = true;
    }

    // ======================================== COMBAT ========================================
    private void OpenSpellMenu()
    {
        //tController.ZA_WARUDO();

    }

    void CheckBlocking()
    {
        if (Input.GetKeyDown(GameMaster.GM.controlManager.blockKey))
        {
            blocking = true;
            tempOriginalStaminaRate = staminaRegenRate;
            staminaRegenRate = guardStaminaRate;
        }// initiates block
        if (Input.GetKeyUp(GameMaster.GM.controlManager.blockKey))
        {
            blocking = false;
            staminaRegenRate = tempOriginalStaminaRate;
        }// ends block
    }// Detemines if blocking should be started or ended, and adjust stamina rates

    public virtual void WhileBlocking()
    {
        // Directional blocking
        if (Input.GetKey(GameMaster.GM.controlManager.upKey))
            animator.Play(BlockUp);
        else
            animator.Play(Block);

        if (Input.GetKeyDown(GameMaster.GM.controlManager.throwKey) || Input.GetKeyDown(GameMaster.GM.controlManager.throwKey2))
        {// A platypus???

            // blocking must remain true for idle to not interrupt parry
            // blocking turns false at the end of the parry animation
            parryWindowOpen = true;
            UseStamina(parryStamina);
            if (Input.GetKey(GameMaster.GM.controlManager.upKey))
                animator.Play(ParryUp); // animation sets parryWindowOpen to true and false
            else
                animator.Play(Parry);

            // Note: add scripts to upper and lower blocks and add OnCollisionEnter2D logic with an if statement for the parry window

        }// PARRY THE PLATYPUS????

    }// Options while blocking

    public void _FinishAttack()
    {
        attacking = false;
        attackConnected = false;
        rb.isKinematic = false;
        //rb.velocity = new Vector2(0, 0);      //causes player to reset momentum when attack finishes
        // allow hitbox1 to hit the same objects again
        hitbox1.ClearNaughtyList();

        staminaRegenRate = tempOriginalStaminaRate; // if stamina is depleted, that's ok

        // recheck whether or not to continue a block
        if (Input.GetKey(GameMaster.GM.controlManager.blockKey) && stamina > 0) // only continue if there is any stamina left
        {
            // temp and real stamina regen rates are already equal here
            blocking = true;
            staminaRegenRate = guardStaminaRate;
            // WhileBlocking() will be called in the Update loop because blocking = true
        }
        else
            blocking = false;

        if (!isGrounded)
        {
            animator.Play(AorUFalling);
        }

    }// End of parry/attack animations

    public void SetUpAttack(float d, float sT, float kM, Vector2 kA) // sets Player attack variables like damage and knockback. ALSO SETS inLag = true;
    {
        // Used in Hitbox1Script OnTriggerEnter2D
        attackDamage = d;
        attackStunTime = sT;
        attackKnockbackMultiplier = kM;
        attackKnockbackAngle = kA;
        attacking = true;
    }

    //public void Attack(GameObject prefabHitBox, float lag, float distance, float height, float sizeX, float sizeY, bool cancellable, int animationHash)
    //{
    //    print("Attack");
    //    animator.Play(animationHash);
    //    attackConnected = false;
    //    // hitBox is an instantiation of hitbox using prefab prefabHitBox. Uses this position and rotation is locked with the player object
    //    GameObject hitBox = Instantiate(prefabHitBox,
    //        new Vector2((faceRight ? 1 : -1) * distance + transform.position.x, height + transform.position.y),
    //        Quaternion.identity);
    //    hitBox.transform.localScale = new Vector2(sizeX, sizeY);      // Size of game object
    //    lightCancellable = cancellable;

    //    // Manage lag
    //    inLag = true;
    //    CancelInvoke(nameof(DisableEndLag)); // Shouldn't ever be necessary if nothing combos into this move
    //    Invoke(nameof(DisableEndLag), lag);
    //}// Attack

    public bool CheckCanThrow()
    {
        if ((Input.GetKeyDown(GameMaster.GM.controlManager.throwKey) || Input.GetKeyDown(GameMaster.GM.controlManager.throwKey2)) && !controlsDisabled && !blocking && !onLedge)
            return true;
        return false;
    }// used to determine if shield can be thrown in Anselm class

    // ======================================== MOVEMENT =======================================
    private void LeftOrRight()
    {
        if (hor < 0 && faceRight) Flip();
        else if (hor > 0 && !faceRight) Flip();
    }

    public void GrabLedge(Vector2 ledgePos)
    {
        rb.isKinematic = true;
        rb.MovePosition(new Vector2(ledgePos.x - (faceRight ? 1 : -1) * ledgeGrabOffset.x, ledgePos.y - ledgeGrabOffset.y));
        ledgeGrabActive = false;
        onLedge = true;
        animator.Play(AorUHanging);
        rb.linearVelocity = Vector2.zero;
        DisableWallJumpBox();
    }

    public void TakeHit(float damage, Vector2 knockback, bool rightOfPlayer)
    {
        if (!invincible)
        {
            blocking = false; // stop blocking
            staminaRegenRate = tempOriginalStaminaRate;
            rb.isKinematic = false; // was true if hit during a time when player is in manual control instead of physics simulation

            AddHealth(-damage); // changes health, updates UI, checks for tired, check for death

            if (!dead) // Die() would have already been run by now
            {
                //invincible = true; // made obolete by use of intagible layer
                // switch to intagible layer
                gameObject.layer = LayerMask.NameToLayer("Player Intangible");
                Invoke(nameof(DisableInvincibility), invincibilityTime); // end I-frames after this many seconds

                // manage knockback
                animator.Play(AorUStunned);
                isStunned = true;
                controlsDisabled = true;
                Invoke(nameof(Unstun), playerStunTime);
                rb.linearVelocity = new Vector2(0, 0);
                rb.AddForce(new Vector2((rightOfPlayer ? -1 : 1) * knockback.x, knockback.y), ForceMode2D.Impulse);

                // slow time
                tController.SetTimeScale(.1f); // uses addition from .1 to 1.0
                camControllerAnimator.SetBool("TimeController_Active", true);
            }

        }// if not invincible
    }

    public void DisableEndLag() // Player regains full control
    {
        jabNum = 0; // reset jab count
        inLag = false;
        attacking = false;
        attackConnected = false;
        lightCancellable = false;
        rb.isKinematic = false;
        rb.linearVelocity = new Vector2(0, 0);
    }

    void DisableLedgeGrabCooldown()
    {
        ledgeGrabCooldown = false;
    }

    public void VerticalNudge()
    {
        if (isFalling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0, verticalNudgeHeight), ForceMode2D.Impulse);
        }
    }// Gives the player a vertical boost while throwing shield if falling

    //void WallJump()
    //{
    //    rb.isKinematic = false;
    //    rb.velocity = wallJumpVelocity;

    //    // ignore horizontal input for a brief moment so that the wall jump always begins with an arc
    //    isIgnoringHorInput = true;
    //    Invoke(nameof(ResetIsIgnoringHorInput), wallJumpLagTime);
    //}

    public void DisableWallJumpBox()
    {
        //wallJumpCheck.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //wallJumpCheck.isWallSliding = false;
        if(!onLedge)
            rb.isKinematic = false;
        ResetIsIgnoringHorInput();
    }

    public void ResetIsIgnoringHorInput() // called by WallJump()
    {
        // allows horizontal input to be used in Update()
        isIgnoringHorInput = false;
    }

    public void Flip()
    {
        transform.Rotate(Vector3.up * 180);
        faceRight = !faceRight;
    }

    void Jump()
    {
        if (stamina > 0)
        {
            rb.linearVelocity = new Vector2(0, 0); // prevents jump forces from accumulating
            rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            landingDust.Stop();
            jumpDust.Play();
        }// check if there is any stamina at all
    }// Jump

    //========================================= HEALTH =========================================
    void SetHealth(float newHealth)
    {
        health = newHealth;
        //Change UI
        //healthBar.SetHealthSlider(health);
    }

    void AddHealth(float addThis) // pass in negative number to subtract health
    {
        health += addThis;
        //Change UI
        //healthBar.SetHealthSlider(health);

        //Health based logic
        if (health <= 0)
            Die();
        else if (health <= .2f * maxHealth)
            DynamicIdle = AorUPant; // reassign idle hash to pant hash
        else
            DynamicIdle = AorUIdle;
    }
    void Die()
    {
        //dead = true;
        AddHealth(maxHealth);// for testing purposes
    }

    //========================================= STAMINA ========================================
    void SetStamina(float newStamina)
    {
        stamina = newStamina;
        //UI changed every frame in fixed update
    }

    public void UseStamina(float staminaUsed)
    {
        stamina -= staminaUsed;
        if (stamina < 0)
            StaminaDepleted();
        //UI changed every frame in fixed update
    }
    public void AddStamina(float staminaAdded) // can be negative, used by depletion/regen rate logic in FixedUpdate
    {
        stamina += staminaAdded;
        if (stamina >= maxStamina)
            StaminaRegenComplete();
        if (stamina <= 0)
            StaminaDepleted();
    }

    void StaminaDepleted()
    {
        staminaRegenRate = depletedStaminaRegenRate;
        tempOriginalStaminaRate = depletedStaminaRegenRate; // blocking logic needs this reset
        blocking = false; // stop blocking
        staminaDepleted = true;
    }

    void StaminaRegenComplete()
    {
        stamina = maxStamina;
        staminaRegenRate = maxStaminaRegenRate;
        tempOriginalStaminaRate = maxStaminaRegenRate; // blocking logic needs this reset
        if (blocking)
            staminaRegenRate = guardStaminaRate;
        staminaDepleted = false;
    }

    bool CheckEnoughStamina(float prospectiveCost)
    {
        if (prospectiveCost < stamina)
            return true;
        else
            return false;
    }// returns true if there is enough stamina remaining


    //========================================= PARTICLE EFFECTS =========================================
    public void CreateLandingDust() // Uses child LandingDust in player prefab
    {
        if (landingDustEnabled) // landingDustEnabled is a variable used in a sketchy way to prevent unwanted dust when GroundCheck box OnEnterTrigger is called
        {
            landingDust.Play();
        }
    }

    public void CreateThrowDustEmitter() // Instantiates a new gameobject, the prefab of throwdust
    {
        ParticleSystem newEmitter = Instantiate(throwDust, transform.position, Quaternion.identity);
        if(!faceRight)
            newEmitter.transform.Rotate(Vector3.up * 180);
        newEmitter.Play();
        Destroy(newEmitter, 3f);
    }    

    //============================ Events for animator ============================
    // physics
    void _Set_isKinematic_False()
    {
        rb.isKinematic = false;
    }
    void _Set_isKinematic_True()
    {
        rb.isKinematic = true;
    }
    // velocities
    void _Set_velocity_Forward()
    {
        rb.linearVelocity = new Vector2((faceRight ? 1 : -1) * 2, 0);
    }
    void _Set_velocity_Upward()
    {
        rb.linearVelocity = new Vector2(0, 4);
    }
    void _Set_velocity_Upward_Slow()
    {
        rb.linearVelocity = new Vector2(0, 2);
    }
    void _Set_velocity_Upward_Fast()
    {
        rb.linearVelocity = new Vector2(0, 8);
    }
    // Unique stuff
    void _Apply_Ledge_Jump_Force()
    {
        rb.isKinematic = false;
        rb.AddForce((staminaDepleted ? 0.75f : 1f) * (new Vector2(/*(faceRight ? 1 : -1) * jumpHeight * 0.1f*/0, jumpHeight * 1.1f)), ForceMode2D.Impulse);
        // Depending on value of                                                        ^this x coordiate^,
        // holding forward boosts the jump distance and holding back hinders the jump
        // It's not a bug, it's a feature
        // Caused by rigidbody keeping the force or velocity. Velocity must be zero'd out again before x-direction movement returns to normal
    }
    void _PlayFallingExt()
    {
        animator.Play(AorUFallingExt);
    }

    void _CompleteLedgeGetup()
    {
        transform.position += new Vector3((faceRight ? 1 : -1) * 0.6f, .9f);
        rb.isKinematic = false;
        _TempDisableLandingDust();// prevents landing dust from starting
    }// called at end of ledge climb animation

    void _CompleteLedgeJump()
    {
        transform.position += new Vector3((faceRight ? 1 : -1) * 0.6f, .9f);
        rb.isKinematic = false;
        _TempDisableLandingDust();// prevents landing dust from starting
        jumpDust.Play();
        UseStamina(jumpStamina);
    }

    void _TempDisableLandingDust() // Not used in any animations but could be
    {
        landingDustEnabled = false;
        Invoke(nameof(EnableLandingDust), 0.1f);
    }
    void EnableLandingDust()
    {
        landingDustEnabled = true; // used for landing dust
    }

    void Test()
    {
        //if (Time.timeScale == 1)
        //{
        //    Time.timeScale = 0.1f;
        //    Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //    Time.fixedDeltaTime = 0.02f;
        //}
    }

}// Player