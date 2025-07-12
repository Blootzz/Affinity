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
    /// DOES NOT ASSUME ANYTHING IS NORMALIZED, DO THAT YOURSELF 
    /// </summary>
    /// <param name="newVelocity"></param>
    public void SetVelocity(Vector2 newVelocity)
    {
        currentVelocity = newVelocity;
    }

}
