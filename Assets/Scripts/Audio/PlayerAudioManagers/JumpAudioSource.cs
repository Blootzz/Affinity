using UnityEngine;

public class JumpAudioSource : MonoBehaviour
{
    [SerializeField] PlayerStateManager stateManager;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        stateManager.playerStateJumping.JumpAudioEvent += OnJumpAudioEvent;
        stateManager.playerStateWallJumping.WallJumpEvent += OnJumpAudioEvent;
    }
    private void OnDisable()
    {
        stateManager.playerStateJumping.JumpAudioEvent -= OnJumpAudioEvent;
        stateManager.playerStateWallJumping.WallJumpEvent -= OnJumpAudioEvent;
    }

    void OnJumpAudioEvent()
    {
        audioSource.Play();
    }
}
