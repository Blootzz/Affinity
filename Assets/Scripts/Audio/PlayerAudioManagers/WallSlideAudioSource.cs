using UnityEngine;

public class WallSlideAudioSource : MonoBehaviour
{
    [SerializeField] PlayerStateManager stateManager;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        stateManager.playerStateWallSlide.StartedSlideEvent += OnSlideAudioEvent;
    }
    private void OnDisable()
    {
        stateManager.playerStateWallSlide.StartedSlideEvent -= OnSlideAudioEvent;
    }

    // ignore faceRight. This needs to be in the event signature for the dust effect
    void OnSlideAudioEvent(bool slideStarted, bool faceRight)
    {
        if (slideStarted)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
