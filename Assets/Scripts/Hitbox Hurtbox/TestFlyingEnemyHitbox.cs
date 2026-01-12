using UnityEngine;

public class TestFlyingEnemyHitbox : MonoBehaviour
{
    [SerializeField] KeyCode TestKey = KeyCode.T;
    [SerializeField] Vector2 Velocity = Vector2.left;
    [SerializeField] Vector2 StartPosition;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(TestKey))
        {
            print("TEST: Moving Hitbox");
            transform.position = StartPosition;
            rb.linearVelocity = Velocity;
        }
    }
}
