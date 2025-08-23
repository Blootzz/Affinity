using UnityEngine;

public class SlideTowardPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    Transform playerTransform;
    Vector2 targetPos;
    float targetXPos;
    float lerpStrength;

    bool slidingActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void BeginSlide(Transform pTransform, float strength)
    {
        playerTransform = pTransform;
        lerpStrength = strength;

        slidingActive = true;
    }
    public void EndSlide()
    {
        slidingActive = false;
    }

    void Update()
    {
        if (!slidingActive)
            return;

        // lerp from here to player by lerpStrength
        targetXPos = Mathf.Lerp(transform.position.x, playerTransform.position.x, lerpStrength * Time.fixedDeltaTime);

        targetPos.x = targetXPos;
        targetPos.y = transform.position.y; // maintain same height
        rb.MovePosition(targetPos);

    }
}
