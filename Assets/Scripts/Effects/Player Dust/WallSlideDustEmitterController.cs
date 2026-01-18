using UnityEngine;

public class WallSlideDustEmitterController : MonoBehaviour
{
    PlayerStateManager stateManager;
    ParticleSystem wallDust;
    ParticleSystemRenderer particleRenderer;

    [SerializeField] float rightPivotY = -0.175f;
    [SerializeField] float leftPivotY = -0.45f;

    private void Awake()
    {
        wallDust = GetComponent<ParticleSystem>();
        particleRenderer = wallDust.GetComponent<ParticleSystemRenderer>();
        stateManager = GetComponentInParent<PlayerStateManager>();
    }

    private void OnEnable()
    {
        stateManager.playerStateWallSlide.StartedSlideEvent += ToggleEffect;
    }
    private void OnDisable()
    {
        stateManager.playerStateWallSlide.StartedSlideEvent -= ToggleEffect;
    }

    void ToggleEffect(bool started, bool faceRight)
    {
        if (!started)
        {
            wallDust.Stop();
            return;
        }

        if (faceRight)
        {
            print("Right");
            // set parameters facing right
            particleRenderer.flip.Set(0, 0, 0);
            particleRenderer.pivot.Set(0, rightPivotY, 0);
        }
        else
        {
            print("Left");
            // set parameters facing left
            particleRenderer.flip.Set(0, 1, 0);
            particleRenderer.pivot.Set(0, leftPivotY, 0);
        }

        wallDust.Play();
    }
}
