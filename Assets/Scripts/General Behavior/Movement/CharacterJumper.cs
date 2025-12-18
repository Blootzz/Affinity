using UnityEngine;

public class CharacterJumper : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpHeight = 10;
    [SerializeField] float ascentGravity = 8;
    [SerializeField] float descentGravity = 3;
    [SerializeField] float shortHopVelocityDivisor = 4;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Always on, even when character is not in jump state
    private void FixedUpdate()
    {
        if (rb.linearVelocityY < 0)
            ApplyDescendingGravity();
    }

    /// <summary>
    /// Sets rb.linearVelocityY to jumpheight and applies ascentGravity
    /// </summary>
    public void BeginJumpAscent()
    {
        rb.linearVelocityY = jumpHeight;
        rb.gravityScale = ascentGravity;
    }

    /// <summary>
    /// Divides positive vertical velocity and applies descentGravity
    /// </summary>
    public void BeginDescending()
    {
        ApplyDescendingGravity();
        if (rb.linearVelocityY > 0)
            rb.linearVelocityY /= shortHopVelocityDivisor;
    }

    /// <summary>
    /// Sets gravity scale to descentGravity
    /// </summary>
    void ApplyDescendingGravity()
    {
        rb.gravityScale = descentGravity;
    }

    #region Double Jump
    [Header("Double Jump")]
    [SerializeField] bool isDoubleJumpAbilityEnabled = true; // editor setting
    [SerializeField] public float doubleJumpVelocity;

    private bool canDoubleJump = true; // logic

    public bool CheckIfDoubleJumpIsPossible()
    {
        if (canDoubleJump && isDoubleJumpAbilityEnabled)
        {
            canDoubleJump = false;
            return true;
        }
        return false;
    }

    public void BeginDoubleJumpAscent()
    {
        rb.linearVelocityY = doubleJumpVelocity;
        rb.gravityScale = ascentGravity;
    }

    public void ResetDoubleJump()
    {
        canDoubleJump = true;
    }
    #endregion
}
