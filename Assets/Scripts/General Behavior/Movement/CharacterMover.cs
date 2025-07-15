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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    /// <summary>
    /// Sets rb.linearVelocity to <paramref name="newHorVelocity"/>
    /// DOES NOT ASSUME ANYTHING IS NORMALIZED, DO THAT YOURSELF 
    /// </summary>
    /// <param name="newHorVelocity"></param>
    public void SetHorizontalVelocity(float newHorVelocity)
    {
        currentHorVelocity = newHorVelocity * speed;
        rb.linearVelocityX = currentHorVelocity;
    }

}
