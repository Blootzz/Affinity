using UnityEngine;

public class LandAudioSource : MonoBehaviour
{
    [SerializeField] GroundCheck groundCheck;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        groundCheck.OnGroundedChanged += OnLandAudioEvent;
    }
    private void OnDisable()
    {
        groundCheck.OnGroundedChanged -= OnLandAudioEvent;
    }

    void OnLandAudioEvent(bool isGrounded)
    {
        if (isGrounded)
            audioSource.Play();
    }
}
