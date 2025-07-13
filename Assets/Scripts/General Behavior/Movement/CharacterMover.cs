using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Vector3 currentVelocity = Vector3.zero;
    [SerializeField] float speed = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + currentVelocity * speed * Time.fixedDeltaTime);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    /// <summary>
    /// Every FixedUpdate, adds <paramref name="newVelocity"/> to position in rb.MovePosition
    /// DOES NOT ASSUME ANYTHING IS NORMALIZED, DO THAT YOURSELF 
    /// </summary>
    /// <param name="newVelocity"></param>
    public void SetVelocity(Vector2 newVelocity)
    {
        currentVelocity = newVelocity;
    }

    /// <summary>
    /// To be used after SetVelocity. Compares CharacterMover currentVelocity to passed in faceRight
    /// </summary>
    /// <returns>true if sprite flip was performed</returns>
    public bool FlipResult(bool wasFacingRight)
    {
        if ((currentVelocity.x > 0 && !wasFacingRight) || (currentVelocity.x < 0 && wasFacingRight))
        {
            transform.Rotate(Vector3.up * 180);
            return true;
        }
        return false;
    }
}
