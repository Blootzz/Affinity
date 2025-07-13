using UnityEngine;

public class CharacterJumper : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        print("setting rb.linearVelocityY = 200");
        rb.linearVelocityY = 200;
    }

}
