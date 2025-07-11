using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpCheck : MonoBehaviour
{
    // collider placed in PlayArea layer

    Player thePlayer;
    Rigidbody2D rb;
    public bool isWallSliding = false;
    public float wallSlideSpeed = -3f;
    public Vector2 wallJumpVelocity = new Vector2(5, 13);
    public float wallJumpLagTime = 0.3f; // after a wall jump, horizontal input will be ignored for this amount of time

    public float wallSlideStaminaPerFrame = 1f;
    public float wallJumpStamina = 15f;

    private Collider2D wallCollider;

    void Start()
    {
        thePlayer = transform.parent.gameObject.GetComponent<Player>();
        rb = thePlayer.GetComponent<Rigidbody2D>();
    }

    void Update() // should only be called when rb.isKinematic = true;
    {
        if (isWallSliding)
        {
            thePlayer.UseStamina(wallSlideStaminaPerFrame);
            if (thePlayer.stamina <= 0)
            {
                rb.isKinematic = false;
                isWallSliding = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }// if out of stamina

            // player is facing the wall while sliding (if sliding on a right wall, faceRight = true)
            if (thePlayer.faceRight && Input.GetKeyDown(GameMaster.GM.controlManager.leftKey) || !thePlayer.faceRight && Input.GetKeyDown(GameMaster.GM.controlManager.rightKey))
            {
                ReleaseWall();
            }// if key is pressed away from the wall
            if (Input.GetKeyDown(GameMaster.GM.controlManager.downKey))
            {
                ReleaseWall();
            }// if key is pressed to fast fall down wall
            if (Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey))
            {
                WallJump();
            }

            // ability to ledge grab while sliding
            if (Input.GetKey(GameMaster.GM.controlManager.ledgeGrabKey))
            {
                thePlayer.animator.Play(thePlayer.AorUReach);
                thePlayer.ledgeGrabActive = true;
                isWallSliding = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
                thePlayer.ledgeGrabActive = false;

        }// isWallSliding = true
        else
        {
            // allow for regrabbing wall after pressing down to fast fall
            if (wallCollider != null)
                if ((Input.GetKeyDown(GameMaster.GM.controlManager.leftKey) || Input.GetKeyDown(GameMaster.GM.controlManager.rightKey)) && gameObject.GetComponent<BoxCollider2D>().IsTouching(wallCollider))
                {
                    GrabWall();
                }
        }// isWallSliding = false
    }

    void ReleaseWall()
    {
        // release wall
        rb.isKinematic = false;
        isWallSliding = false;
        thePlayer.GetComponent<Animator>().Play(thePlayer.AorUFalling);
    }

    public void OnTriggerEnter2D(Collider2D collision) // needs to be called again when isFalling turns true
    {
        if (thePlayer.stamina > 0 && collision.gameObject.name.CompareTo("Collision_Default") == 0 /*&& !thePlayer.onLedge && !thePlayer.ledgeGrabActive*/)
        {
            wallCollider = collision.gameObject.GetComponent<Collider2D>();
            GrabWall();
        }// if colliding with wall
    }

    void GrabWall()
    {
        isWallSliding = true;
        rb.isKinematic = true;
        rb.linearVelocity = new Vector2(0, wallSlideSpeed); // wallSlideSpeed is a private float that is already negative
        thePlayer.GetComponent<Animator>().Play(thePlayer.AorUWallSlide);

        // reset double jump
        gameObject.GetComponentInParent<DoubleJump>().ResetDoubleJump();
    }

    public void OnTriggerExit2D(Collider2D collision) // so player returns to falling after sliding off an overhang
    {
        // rb.isKinematic is true only when sliding off overhang
        if (rb.isKinematic && collision.gameObject.name.CompareTo("Collision_Default") == 0 /*&& !thePlayer.onLedge && !thePlayer.ledgeGrabActive*/)
        {
            ReleaseWall();
            rb.linearVelocity = Vector2.zero; // wallSlideSpeed is a private float that is already negative
        }// if colliding with wall
    }

    void WallJump()
    {
        rb.isKinematic = false;
        // set velocity according to direction
        rb.linearVelocity = new Vector2((thePlayer.faceRight ? -1 : 1) * wallJumpVelocity.x, wallJumpVelocity.y);

        // ignore horizontal input for a brief moment so that the wall jump always begins with an arc
        thePlayer.isIgnoringHorInput = true;
        CancelInvoke(nameof(ResetIsIgnoringHorInput));
        Invoke(nameof(ResetIsIgnoringHorInput), wallJumpLagTime);
        isWallSliding = false;

        thePlayer.GetComponent<Animator>().Play(thePlayer.AorUFalling);
        thePlayer.Flip(); // so player faces away from wall

        // stamina
        thePlayer.UseStamina(wallJumpStamina);
    }

    void ResetIsIgnoringHorInput() // called by WallJump()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // makese it so that player does not maintain inertia when taking over controls in the air
        thePlayer.ResetIsIgnoringHorInput();
        // allows horizontal input to be used in Update()
        //thePlayer.isIgnoringHorInput = true;
    }
}
