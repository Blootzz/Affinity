using UnityEngine;

public class CharacterJumper : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DoJump()
    {
        print("setting rb.linearVelocityY = 20");
        rb.linearVelocityY = 20;
    }

}
