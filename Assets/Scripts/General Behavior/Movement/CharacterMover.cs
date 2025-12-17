using UnityEngine;
using System;

public class CharacterMover : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float currentHorVelocity = 0;
    [SerializeField] float speed = 0;
    bool isMoving = false;

    public event Action HorVelocityHitZeroEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Math.Abs(rb.linearVelocityX) > 0.001f)
            isMoving = true;
        else if (Math.Abs(rb.linearVelocityX) < 0.001f && isMoving)
        {
            HorVelocityHitZeroEvent?.Invoke();
            isMoving = false;
        }
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

    public void SetRbType(RigidbodyType2D newType)
    {
        rb.bodyType = newType;
        //print("Switching RB type to: "+newType.ToString());
    }

    /// <summary>
    /// Not using MovePosition, just rb.transform.position = <param name="newPos"></param>
    /// This forces the rigidbody to the position
    /// Good for teleporting regardless of physics
    /// </summary>
    public void SetRBPosition(Vector2 newPos)
    {
        rb.transform.position = newPos;
    }

}
