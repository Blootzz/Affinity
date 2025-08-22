using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float currentHorVelocity = 0;
    [SerializeField] float speed = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //// Do not use this if relying on Unity's physics system to handle gravity, collisions, etc
    //private void FixedUpdate()
    //{
    //    rb.MovePosition(transform.position + currentVelocity * speed * Time.fixedDeltaTime);
    //}

    public void SetMoveSpeed(float newMoveSpeed)
    {
        speed = newMoveSpeed;
    }

    /// <summary>
    /// Sets rb.linearVelocity to <paramref name="newHorVelocity"/> * speed. 
    /// DOES NOT ASSUME ANYTHING IS NORMALIZED, DO THAT YOURSELF
    /// </summary>
    /// <param name="newHorVelocity"></param>
    public void SetHorizontalMovementVelocity(float newHorVelocity)
    {
        currentHorVelocity = newHorVelocity * speed;
        rb.linearVelocityX = currentHorVelocity;
    }

    /// <summary>
    /// Similar to rb.AddForce but does not take mass into account. 
    /// </summary>
    /// <param name="force"></param>
    public void SetVelocity(Vector2 velocity)
    {
        currentHorVelocity = velocity.x;
        rb.linearVelocity = velocity;
    }
    public void SetVelocityX(float newVelocityX)
    {
        currentHorVelocity = newVelocityX;
        rb.linearVelocityX = currentHorVelocity;
    }

}
