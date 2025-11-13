using UnityEngine;
using UnityEngine.UIElements;

public class SlideTowardPlayer : MonoBehaviour
{
    [Header("Can also be used for enemy block slide")]
    Rigidbody2D rb;
    Transform playerTransform;
    Vector2 resultTargetPos;
    float targetXPos;
    float lerpStrength;
    [SerializeField] float baseLerpMultiplier = 1;

    Vector2 fixedTargetPos;

    bool slidingActive = false;
    bool slideByTransform = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<Animator>().applyRootMotion == false)
            Debug.LogWarning("Animator.applyRootMotion must be true or it will override any change in position. 2 hours lost to this quirk");
    }

    public void BeginSlide(Transform pTransform, float strength)
    {
        playerTransform = pTransform;
        slideByTransform = true;

        lerpStrength = strength;

        slidingActive = true;
    }
    public void BeginSlide(float distance, bool moveRight, float strength)
    {
        fixedTargetPos = new Vector2(
            transform.position.x + (distance * (moveRight ? -1 : 1)),
            transform.position.y);
        slideByTransform = false;

        lerpStrength = strength;

        slidingActive = true;
    }

    public void EndSlide()
    {
        slidingActive = false;
    }

    // hopefully having rb.interpolate allows smooth movement
    void FixedUpdate()
    {
        if (!slidingActive)
            return;

        if (slideByTransform)
        {
            // lerp from here to player by lerpStrength
            targetXPos = Mathf.Lerp(transform.position.x, playerTransform.position.x, lerpStrength * Time.fixedDeltaTime * baseLerpMultiplier);
            //print("lerp strength: " + (lerpStrength * Time.fixedDeltaTime * baseLerpMultiplier));

            // bundle into Vector2 targetPos
            resultTargetPos.x = targetXPos;
            resultTargetPos.y = transform.position.y; // maintain same height
        }
        else // only lerping to fixed position, not a specific transform
            resultTargetPos = fixedTargetPos;

        rb.MovePosition(resultTargetPos);

    }
}
