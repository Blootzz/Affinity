using UnityEngine;
using System;

public class CharacterMover : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float currentHorVelocity = 0;
    [SerializeField] float characterSpeed = 0;
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
        characterSpeed = newMoveSpeed;
    }

    /// <summary>
    /// Sets rb.linearVelocityX to <paramref name="newHorVelocity"/> * characterSpeed. 
    /// If you don't want to involve characterSpeed, use SetVelocityX
    /// </summary>
    public void SetHorizontalMovementVelocity(float newHorVelocity)
    {
        currentHorVelocity = newHorVelocity * characterSpeed;
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

    /// <summary>
    /// Directly sets rb.linearVelocityX to <paramref name="newVelocityX"/>
    /// bypasses characterSpeed
    /// </summary>
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
