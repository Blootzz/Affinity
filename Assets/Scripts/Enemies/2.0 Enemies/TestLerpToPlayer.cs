using UnityEngine;

public class TestLerpToPlayer : MonoBehaviour
{
    [SerializeField] Vector2 testPos;
    [SerializeField] float lerpStrength = 0.1f;

    Vector2 resultTargetPos;
    Rigidbody2D rb;
    float targetXPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        // lerp from here to player by lerpStrength
        targetXPos = Mathf.Lerp(transform.position.x, testPos.x, lerpStrength /** Time.fixedDeltaTime * baseLerpMultiplier*/);
        //print("lerp strength: " + (lerpStrength * Time.fixedDeltaTime * baseLerpMultiplier));

        // bundle into Vector2 targetPos
        resultTargetPos.x = targetXPos;
        resultTargetPos.y = transform.position.y; // maintain same height
        print("resultTargetPos: " + resultTargetPos);

        rb.MovePosition(resultTargetPos);

    }
}
