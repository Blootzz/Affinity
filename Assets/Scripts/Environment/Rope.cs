using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    float ropeSpriteHeight = 0.175f;
    Vector2 knotToKnotDistance;

    float verticalIndent; // variable calculated to find how low player should be on rope
    public float indentMultiplier_RopeLength = .15f; // scales how much total rope length increases verticalIndent
    public float indentMultiplier_NearestKnot = .15f; // scales how much proximity away from nearest anchor increases verticalIndent

    // player movement
    [SerializeField]
    float horizontalSpeed = .1f;
    bool playerIsOnRope = false;
    bool attackForward = true; // used to determine which way to attack when the attack button is clicked

    public GameObject leftRope;
    public GameObject rightRope;

    GameObject leftLength;
    GameObject leftKnot;

    GameObject rightLength;
    GameObject rightKnot;

    Player thePlayer;
    GameObject groundCheck;

    void Start()
    {
        // get reference to player
        thePlayer = FindObjectOfType<Player>();
        groundCheck = thePlayer.GetComponentInChildren<Grounded>().gameObject;

        // get references to each component
        leftLength = leftRope.transform.GetChild(0).gameObject;
        leftKnot = leftRope.transform.GetChild(1).gameObject;
        
        rightLength = rightRope.transform.GetChild(0).gameObject;
        rightKnot = rightRope.transform.GetChild(1).gameObject;

        knotToKnotDistance = rightKnot.transform.position - leftKnot.transform.position; // finds straight distance between knots

        SetCollider(); // sets collider to accurately represent rope orientation
        RevertSpritesToNormal(); // call this to set correct angle and lengths
    }

    void SetCollider() // only called one time to properly resize collider
    {
        // create array with 2 entries {leftKnot position, rightKnot position}
        // (must add local positions of all levels of parent objects to get correct local position relative to top parent)
        Vector2[] endPoints = {leftKnot.transform.localPosition + leftRope.transform.localPosition, rightKnot.transform.localPosition + rightRope.transform.localPosition};

        // sec collider points to knot positions. POINT COORDINATES ARE RELATIVE TO TOP PARENT OBJECT, since this component belongs to the top parent
        gameObject.GetComponent<EdgeCollider2D>().points = endPoints;
    }

    void RevertSpritesToNormal() // change angle of leftLength to point towards rightKnot, and extend leftLength to rightKnot. rightLength is zero units long
    {
        // find defaultAngle from leftKnot to rightKnot
        float defaultAngle = FindAngleBetween(leftKnot.transform.position, rightKnot.transform.position);

        // rotate leftLength
        leftLength.transform.eulerAngles = new Vector3(0, 0, defaultAngle);  

        // extend leftLength
        leftLength.GetComponent<SpriteRenderer>().size = new Vector2(knotToKnotDistance.magnitude, ropeSpriteHeight); // sets length of leftLength to reach goal
        // shrink rightLength
        rightLength.GetComponent<SpriteRenderer>().size = new Vector2(0, ropeSpriteHeight); // shrinks rightLength to nothingness
    }

    void RedrawRopeWithIndent()
    {

        // rotate leftLength
        // indent already taken into account by player position
        float leftAngle = FindAngleBetween(leftKnot.transform.position, thePlayer.transform.position);
        leftLength.transform.eulerAngles = new Vector3(0, 0, leftAngle);
        // extend leftLength to magnitude of player.position - leftKnot.position
        leftLength.GetComponent<SpriteRenderer>().size = new Vector2((thePlayer.transform.position-leftKnot.transform.position).magnitude, ropeSpriteHeight);

        // rotate rightLength
        float rightAngle = FindAngleBetween(rightKnot.transform.position, thePlayer.transform.position);
        rightLength.transform.eulerAngles = new Vector3(0, 0, rightAngle);
        // extend rightLength
        rightLength.GetComponent<SpriteRenderer>().size = new Vector2((rightKnot.transform.position - thePlayer.transform.position).magnitude, ropeSpriteHeight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!thePlayer.GetComponent<Anselm>().playerShield.shieldDeployed && collision.gameObject.name.CompareTo("LedgeGrabCheck") == 0)
        {
            thePlayer.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            thePlayer.GetComponent<Rigidbody2D>().isKinematic = true;
            thePlayer.isBusy = true;
            playerIsOnRope = true;
            thePlayer.controlsDisabled = true;
            groundCheck.SetActive(false); // prevents ground check from hitting wall and registering player as grounded (changing rigidbody to dynamic again)

            //place player exactly on path
            float proportionalHeight = leftKnot.transform.position.y // based off left knot for world space
                    + (thePlayer.transform.position.x - leftKnot.transform.position.x) * (knotToKnotDistance.y / knotToKnotDistance.x); // proportionality from player distance from left knot
                                                                                                                                        // determine Y value for player to move to
            CalculateVerticalIndent();
            float newPlayerY = proportionalHeight - verticalIndent;
            thePlayer.transform.position = new Vector2(thePlayer.transform.position.x, newPlayerY);
            print("Placing Player at: " +newPlayerY);
        }
    }

    private void FixedUpdate()
    {
        // if on rope, move player according to horizontal input and indent
        if (playerIsOnRope)
        {
            // These variables in combination with the "else" statement prevent player from sliding straight down at ends
            Vector2 oldPosition = thePlayer.transform.position;

            //  horizontal input
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if (!thePlayer.attacking)
                DoZiplineAnimations(horizontalInput);

            // determine X value for player to move to
            float newPlayerX = thePlayer.transform.position.x + horizontalInput * horizontalSpeed * Time.timeScale;

            // only proceed if within bounds of knots
            if (newPlayerX > leftKnot.transform.position.x && newPlayerX < rightKnot.transform.position.x)
            {
                // determine proportionalHeight (a.k.a. what the vertical component of the player should be on if the rope was perfectly taught)
                float proportionalHeight = leftKnot.transform.position.y // based off left knot for world space
                    + (thePlayer.transform.position.x - leftKnot.transform.position.x) * (knotToKnotDistance.y / knotToKnotDistance.x); // proportionality from player distance from left knot

                // determine Y value for player to move to
                CalculateVerticalIndent();
                float newPlayerY = proportionalHeight - verticalIndent;


                // update player position
                thePlayer.transform.position = new Vector3(newPlayerX, newPlayerY);
                // update rope sprites
                RedrawRopeWithIndent();
            }// if within knots
            else
            {
                thePlayer.transform.position = oldPosition;
            }

        }// if playerIsOnRope
    }// FixedUpdate()

    private void Update()
    {
        if (playerIsOnRope && !thePlayer.attacking)
        {
            if (Input.GetKeyDown(GameMaster.GM.controlManager.ledgeGrabKey))
                ReleaseRope();
            if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey))
                JumpOffRope(); // calls ReleaseRope()
            if (Input.GetKeyDown(GameMaster.GM.controlManager.attackKey))
                DoAttack();
        }
    }

    void DoAttack()
    {
        if (attackForward)
            thePlayer.animator.Play(thePlayer.ZipAttackForward);
        else
            thePlayer.animator.Play(thePlayer.ZipAttackBackward);
    }

    void ReleaseRope()
    {
        playerIsOnRope = false;
        thePlayer.isBusy = false;

        // reset rope
        RevertSpritesToNormal();

        // fix player data
        thePlayer.controlsDisabled = false;
        groundCheck.SetActive(true);
        thePlayer.GetComponent<Rigidbody2D>().isKinematic = false;
        thePlayer.animator.Play(thePlayer.AorUFalling);
    }

    void JumpOffRope()
    {
        ReleaseRope();
    }

    void DoZiplineAnimations(float hor)
    {
        if (hor == 0)
        {
            thePlayer.animator.Play(thePlayer.ZiplineForward);
        }
        else
        {
            if (thePlayer.faceRight)
            {
                if (hor > 0)
                {
                    thePlayer.animator.Play(thePlayer.ZiplineForward);
                    attackForward = true;
                }
                if (hor < 0)
                {
                    thePlayer.animator.Play(thePlayer.ZiplineBackward);
                    attackForward = false;
                }
            }
            else // facing left
            {
                if (hor > 0)
                {
                    thePlayer.animator.Play(thePlayer.ZiplineBackward);
                    attackForward = false;
                }
                if (hor < 0)
                {
                    thePlayer.animator.Play(thePlayer.ZiplineForward);
                    attackForward = true;
                }
            }
        }    
    }

    void CalculateVerticalIndent()
    {
        float distanceToNearestKnot;
        // find x distance to nearest knot
        if (thePlayer.transform.position.x - leftKnot.transform.position.x < knotToKnotDistance.x / 2) // if player is on left side of rope
            distanceToNearestKnot = thePlayer.transform.position.x - leftKnot.transform.position.x;
        else
            distanceToNearestKnot = -1 * (thePlayer.transform.position.x - rightKnot.transform.position.x); // player.x - rightKnot.x will be negative so it is multiplied by -1

        verticalIndent = (distanceToNearestKnot * indentMultiplier_NearestKnot) * (knotToKnotDistance.x * indentMultiplier_RopeLength);
    }

    float FindAngleBetween(Vector2 startingPoint, Vector2 targetPoint)
    {
        Vector2 difference = targetPoint - startingPoint;
        return Mathf.Rad2Deg * Mathf.Atan2(difference.y, difference.x); // returns angle of difference vector
        // Atan2 takes into account all quadrants of angles, while normal tan loses information
    }

}
