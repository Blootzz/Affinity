using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shield : MonoBehaviour
{
    Animator animator;
    Player thePlayer;
    SpriteRenderer sr;
    Rigidbody2D rb;

    public float throwSpeed = .3f;
    public float lift; // declared in Start so lift can depend on throwSpeed
    [HideInInspector]
    public float deceleration = .01f;
    [HideInInspector]
    public float acceleration = .01f;
    public float throwDamage = 20;
    public float knockbackMultiplier = 1;
    public float stunTime = 0.2f;
    public bool movingAway = true; // set true even if holding shield, Only false when shield is supposed to come back
    public bool shieldDeployed = false;
    public bool shootRight = true;

    // Ability stuff
    bool isAbilityToggled = false;
    public bool cannotActivateAgain = false;
    public int selectedAbilityIndex = 0; // determines what ability the shield does in the air
    public bool isPlatform = false;
    public bool isPlayerHangingOnPlatform = false; // Used by ShieldLedge
    //float platformStamina = 20f;

    // movement
    int originalLayer; // usual layer for shield, changes to player's layer when a platform
    float Px = 0;
    float Py = 0;
    int frameCount = 0; // number of frames the shield is deployed
    int stopFrame = 0; // the frame number that the shield reverses direction
    bool flipNextFrame = false; // used to avoid collision thread messing up update thread- flips direction of shield on collision
    float xPos; // Used to figure out angle of path for tilting shield
    float yPos; // Same as above

    //Animations
    private int SpinFlat;
    private int SpinAngleUp;
    private int SpinAngleDown;
    private int Idle;

    private void Awake()
    {
        selectedAbilityIndex = 0;
        SceneManager.sceneLoaded += ReturnShieldOnLoad;
        // returns the shield and resets variables when moving to new scene
        // used when shield is deployed and player enters load zone
    }

    // Start is called before the first frame update
    void Start()
    {
        lift = throwSpeed / 2;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false; // Don't let shield collide with player
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        originalLayer = gameObject.layer;

        SpinFlat = Animator.StringToHash("ShieldSpinFlat");
        SpinAngleUp = Animator.StringToHash("ShieldSpinAngleUp");
        SpinAngleDown = Animator.StringToHash("ShieldSpinAngleDown");
        Idle = Animator.StringToHash("ShieldIdle");
        thePlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Regular shield movement
        if (!isAbilityToggled)
        {
            Px = thePlayer.transform.position.x;
            Py = thePlayer.transform.position.y + thePlayer.offsetY;

            if (!shieldDeployed)
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            else
                TrackTrajectory();

            if (flipNextFrame) // Keeps manipulation of movingAway in this thread (OnCollisionEnter2D shouldn't be used because of this)
            {
                movingAway = false;
                stopFrame = frameCount;
                flipNextFrame = false;
            }
        }

    }// UPDATE

    private void OnCollisionEnter2D(Collision2D collision) // bounces back after hitting another collider that isn't the player
    {
        if (shieldDeployed && !isPlatform)
        {
            if (!collision.gameObject.CompareTag("Player"))
                CollideWithObject(collision); //if colliding with anything but the player
        }// shield is deployed

    }// Enters a Collider 2D

    void CollideWithObject(Collision2D collision)
    {
        BeginToComeBack();

        // collisions with kinematically controlled objects like Projectile.cs are handled in those scripts
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Breakable Object"))
        {
            float xc; float yc;
            xc = collision.gameObject.transform.position.x; // returns the center of the enemy or breakable object IF TRANSFORM.POSITION IS CALCULATED AT CENTER
            yc = collision.gameObject.transform.position.y;
            Vector2 launchVector = new Vector2(xc - this.transform.position.x, yc - this.transform.position.y);

            // negate x if on other side of player. Gets negated again in TakeDamage
            if (collision.gameObject.CompareTag("Enemy"))
            {
                launchVector = new Vector2((collision.gameObject.GetComponent<Enemy>().rightOfPlayer ? 1 : -1) * launchVector.x, launchVector.y);
                //accesses "Enemy" script of object and calls TakeDamage
                collision.gameObject.GetComponent<Enemy>().TakeDamage(throwDamage, stunTime, knockbackMultiplier, launchVector);
            }

            if (collision.gameObject.CompareTag("Breakable Object"))
            {
                //launchVector = new Vector2((collision.gameObject.GetComponent<Breakable Object>().rightOfPlayer ? 1 : -1) * launchVector.x, launchVector.y);
                //accesses script of object and calls TakeDamage
                //collision.gameObject.GetComponent<BreakableObject>().TakeDamage(throwDamage, knockbackMultiplier, stunTime, launchVector);
            }
        }// deal damage
    }// CollideWithObject given collision

    public void BeginToComeBack()
    {
        flipNextFrame = true; // flips the direction of the shield
        gameObject.layer = LayerMask.NameToLayer("Shield Return"); // changes physics layer of shield
    }

    public void Yeet(float playerSpeedY, float extraMultiplier) //playerSpeed.x will be zero if there is no rigidBody force acting in the x direction
    {
        if (!shieldDeployed)
        {
            // start in front of player
            transform.position = new Vector2(thePlayer.transform.position.x + (thePlayer.faceRight ? 1 : -1) * .3f, thePlayer.transform.position.y + thePlayer.offsetY);
            // make visible and tangible
            GetComponent<CircleCollider2D>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;

            shieldDeployed = true;
            shootRight = thePlayer.faceRight; // shootRight will stay constant throughout the entire flight while thePlayer.faceRight may change

            if (shootRight)
            {
                animator.Play(SpinFlat);
            }
            else
            {
                animator.Play(SpinFlat);
            }
        }
    }// yeet shield

    void Blink()
    {
        if (sr.enabled)
            sr.enabled = false;
        else
            sr.enabled = true;
    }

    private void TrackTrajectory()
    {
        xPos = transform.position.x; // initial shield position used for tilt later
        yPos = transform.position.y;

        // only returning logic is adjusted for time change
        if (movingAway)
        {
            float verticalStep = 0;
            if (Input.GetKey(GameMaster.GM.controlManager.upKey))
                verticalStep = lift;
            else if (Input.GetKey(GameMaster.GM.controlManager.downKey))
                verticalStep = -lift;

            // "frameCount" is actually only updated in FixedUpdate() so there won't be any framerate issues
            float horizontalBonus = (verticalStep == 0 ? 1.3f : 1f); // gives a 1.3x horizontal distance increase if no lift is applied (results in a more circular range)
            Vector3 step = new Vector2((shootRight ? 1 : -1) * horizontalBonus * (throwSpeed - (deceleration * frameCount)), verticalStep);
            transform.position += Time.timeScale * step; // affected by slow motion
            if (shootRight && step.x < 0f || !shootRight && step.x > 0f) //when the change in position is zero
            {
                movingAway = false;
                stopFrame = frameCount;
            }// check to see if still moving away
        }// Moving Away == true
        else
        {
            //disable collision with everything except the player and enemies

            float shieldMinusPlayerX = transform.position.x - Px;
            float shieldMinusPlayerY = transform.position.y - Py;

            if (Mathf.Abs(shieldMinusPlayerX) < 1 && Mathf.Abs(shieldMinusPlayerY) < 1)
            {
                ShieldCaught();
            }//if close
            else
            {
                float verticalStep = 0;
                if (Input.GetKey(GameMaster.GM.controlManager.upKey))
                    verticalStep = lift;
                else if (Input.GetKey(GameMaster.GM.controlManager.downKey))
                    verticalStep = -lift;

                Vector3 difference = new Vector3(
                    x: shieldMinusPlayerX * acceleration * (frameCount - stopFrame),
                    y: shieldMinusPlayerY * acceleration * (frameCount - stopFrame) - verticalStep*1.2f);
                // adjusted for time changing shenanigains
                //transform.position -= difference * Time.fixedDeltaTime / thePlayer.GetComponent<TimeController2>().originalFixedDeltaTime;
                transform.position -= difference * Time.timeScale;
            }// Shield needs to move closer

        }//Moving Away == false

        {// Shield has been moved either away from or towards player since xPos and yPos recorded
         // In a 30, 60, 90 triangle where dy is shorter than dx, dy = dx * 0.578 or [dy = dx * (1/sqrt(3))]
            double dx = transform.position.x - xPos;
            double dy = transform.position.y - yPos;
            if (Mathf.Abs((float)dy) > Mathf.Abs((float)dx))
            {
                animator.Play(SpinFlat);
            }
            else
            {
                if (dy > 0)
                {
                    if (dx > 0)
                    {
                        if (dy > 0.578 * dx)
                        {
                            animator.Play(SpinAngleUp);
                        }
                    }// first quadrant
                    else // dx <= 0
                    {
                        if (dy > (-0.578 * dx))
                        {
                            animator.Play(SpinAngleDown);
                        }
                    }// second quadrant
                }// first two quadrants
                else
                {
                    if (dx > 0)
                    {
                        if (dy < (-0.578 * dx))
                        {
                            animator.Play(SpinAngleDown);
                        }
                    }// fourth quadrant
                    else // dx <= 0
                    {
                        if (dy < (0.578 * dx))
                        {
                            animator.Play(SpinAngleUp);
                        }
                    }// third quadrant
                }// last two quadrants
            }// adjustment needed
        }// shield tilt logic

        frameCount += 1; // Used in fixed update so this really just means # of calls of FixedUpdate
    }// TrackTrajectory()

    void ShieldCaught()
    {
        gameObject.layer = LayerMask.NameToLayer("Shield"); // instead of shield return
        shieldDeployed = false;
        movingAway = true;
        cannotActivateAgain = false;
        GetComponent<CircleCollider2D>().enabled = false;
        frameCount = 0;
        animator.Play(Idle);
        thePlayer.ToggleAorUAnimations();
        if (!thePlayer.onLedge)
        {
            if (thePlayer.isGrounded)
                thePlayer.animator.Play(thePlayer.Idle);
            else
                thePlayer.animator.Play(thePlayer.Falling);
        }
        else
            thePlayer.animator.Play(thePlayer.AorUHanging);
    }

    void ReturnShieldOnLoad(Scene scene, LoadSceneMode mode)
    {
        if (shieldDeployed)
        {
            _DeactivateAbility();

            // shield caught logic
            gameObject.layer = LayerMask.NameToLayer("Shield"); // instead of shield return
            shieldDeployed = false;
            movingAway = true;
            cannotActivateAgain = false;
            GetComponent<CircleCollider2D>().enabled = false;
            frameCount = 0;
            animator.Play(Idle);
            thePlayer.ToggleAorUAnimations();
            transform.position = thePlayer.transform.position;
        }
    }

    public void _Begin_Blinking()
    {
        InvokeRepeating(nameof(Blink), 0f, .1f);
    }
    
    // ========================= Abilities =========================
    public void EngageShieldAbility()
    {
        if (!isAbilityToggled)
        {
            if (thePlayer.stamina > 0)
            {
                ActivateAbility();
            }
        }// not yet toggled
        else
        {
            // this will be called in shield animator if player doesn't recall shield quickly
            _DeactivateAbility();
        }// already a platform

    }

    void ActivateAbility()
    {
        if (!cannotActivateAgain)
        {
            // this executes OnEnable for the ability-specific scripts
            gameObject.transform.GetChild(selectedAbilityIndex).gameObject.SetActive(true);

            isAbilityToggled = true;
            cannotActivateAgain = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            sr.sortingLayerName = "Player";
            gameObject.layer = thePlayer.gameObject.layer;
        }
    }

    // ========================== Animation Events used in ability-specific animations ==========================

    public void _DeactivateAbility()
    {
        //gameObject.transform.GetChild(selectedAbilityIndex).gameObject.SetActive(false);

        // deactivate all children so that there is no shenanigans (don't make this dependent on selectedAbilityIndex because that can change any time)
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        isAbilityToggled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        gameObject.layer = originalLayer; // sets to regular collision layer shield is used on
        gameObject.tag = "Shield";
        CancelInvoke(nameof(Blink));
        sr.sortingLayerName = "Shield";
        sr.enabled = true; // ensures not left false by Blink()
        animator.Play(SpinFlat);
    }

}//Shield
