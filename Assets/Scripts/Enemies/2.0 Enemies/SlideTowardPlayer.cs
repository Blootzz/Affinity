using UnityEngine;

public class SlideTowardPlayer : MonoBehaviour
{
    [SerializeField] PhysicsMaterial2D zeroFrictionBounce;
    [SerializeField] PhysicsMaterial2D slideMaterial;

    Rigidbody2D rb;
    Transform playerTransform;
    Vector2 resultTargetPos;
    float targetXPos;
    float lerpStrength;
    [SerializeField] float baseLerpMultiplier = 10;

    bool slidingActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void BeginSlide(Transform pTransform, float strength)
    {
        rb.sharedMaterial = slideMaterial;

        playerTransform = pTransform;
        lerpStrength = strength;

        slidingActive = true;
    }
    public void EndSlide()
    {
        slidingActive = false;
        rb.sharedMaterial = zeroFrictionBounce;
    }

    void Update()
    {
        if (!slidingActive)
            return;

        // lerp from here to player by lerpStrength
        targetXPos = Mathf.Lerp(transform.position.x, playerTransform.position.x,lerpStrength /** Time.fixedDeltaTime * baseLerpMultiplier*/);
        //print("lerp strength: " + (lerpStrength * Time.fixedDeltaTime * baseLerpMultiplier));

        // bundle into Vector2 targetPos
        resultTargetPos.x = targetXPos;
        resultTargetPos.y = transform.position.y; // maintain same height
        print("resultTargetPos: " + resultTargetPos);

        rb.MovePosition(resultTargetPos);

    }
}
