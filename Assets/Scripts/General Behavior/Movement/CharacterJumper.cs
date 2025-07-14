using UnityEngine;

public class CharacterJumper : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpHeight = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
        rb.linearVelocityY = jumpHeight;
    }

}
