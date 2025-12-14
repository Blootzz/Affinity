using UnityEngine;

public class JumpLandDustFXManager : MonoBehaviour
{
    [SerializeField] GameObject jumpDust;
    [SerializeField] GameObject landingDust;
    GroundCheck groundCheck;

    private void Awake()
    {
        groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable()
    {
        groundCheck.OnGroundedChanged += EnableLandingDust;
    }
    private void OnDisable()
    {
        groundCheck.OnGroundedChanged -= EnableLandingDust;
    }

    void EnableLandingDust(bool isGrounded)
    {
        // jump dust should only go off with jump command, not every time GroundCheck exits an object
        if (isGrounded)
            landingDust.SetActive(true);
    }

    public void EnableJumpDust()
    {
        jumpDust.SetActive(true);
    }
}
