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
        print("Something in this script is causing enemy to freeze in air after being parried");
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
    public void BeginSlide(float distance, bool faceRight, float strength)
    {
        fixedTargetPos = new Vector2(
            transform.position.x + (distance * (faceRight ? -1 : 1)),
            transform.position.y);
        slideByTransform = false;

        lerpStrength = strength;

        slidingActive = true;
    }
    // force isn't working and not sure why, just keep using velocity
    //public void BeginSlide(float forceMagnitude, bool faceRight)
    //{
    //    EndSlide(); // set slidingActive to false so that can sliding toward player can be interrupted
    //    print("Adding force of: "+ new Vector2(forceMagnitude * (faceRight ? -1 : 1), 10));
    //    rb.AddForce(new Vector2(forceMagnitude * (faceRight ? -1 : 1), 10));

    //    slideByForce = true;
    //}

    public void EndSlide()
    {
        slidingActive = false;
    }

    // hopefully having rb.interpolate allows smooth movement
    void FixedUpdate()
    {
        if (!slidingActive)
            return;

        //print("Sliding fixed update");

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
