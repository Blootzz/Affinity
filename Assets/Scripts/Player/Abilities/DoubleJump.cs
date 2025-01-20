using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleJump : MonoBehaviour
{
    [SerializeField]
    private bool canDoubleJump = true;
    [SerializeField]
    private float doubleJumpHeight = 5f;
    UnityEvent m_MyEvent;

    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

        gameObject.GetComponentInChildren<Grounded>().OnGroundedExit += ResetDoubleJump;
    }

    private void Update()
    {
        if (!gameObject.GetComponent<Player>().controlsDisabled) // controls are enabled
            if (canDoubleJump && Input.GetKeyDown(GameMaster.GM.controlManager.jumpKey) && !gameObject.GetComponent<Player>().isGrounded) // double jump basic conditions
                if (!gameObject.GetComponentInChildren<WallJumpCheck>().isWallSliding && !gameObject.GetComponent<Player>().isIgnoringHorInput) // don't double jump when wall jump is available
                {
                    DoDoubleJump();
                }
    }

    void DoDoubleJump()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        canDoubleJump = false;
        rb.velocity = new Vector2(0, 0); // prevents jump forces from accumulating
        rb.AddForce(new Vector2(0f, doubleJumpHeight), ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        gameObject.GetComponentInChildren<Grounded>().OnGroundedExit -= ResetDoubleJump; // error here sometimes caused by dismounting rope???
    }// prevents errors

    public void ResetDoubleJump()
    {
        canDoubleJump = true;
    }
}
