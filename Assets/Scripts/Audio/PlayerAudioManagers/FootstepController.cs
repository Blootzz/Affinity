using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [SerializeField] AudioClip[] footstepClips;
    [SerializeField] float stepSeconds = 0.4f;
    [SerializeField] SFXPool sfxPool;

    float timer; // used to determine when stepSeconds has been reached
    
    PlayerStateManager stateManager;
    bool isInRunningState = false;

    private void OnEnable()
    {
        stateManager.playerStateRunning.StartedRunningEvent += OnRunningStateChanged;
    }
    private void OnDisable()
    {
        stateManager.playerStateRunning.StartedRunningEvent -= OnRunningStateChanged;
    }

    void Update()
    {
        if (!isInRunningState)
            return;

        timer += Time.deltaTime;
        if (timer >= stepSeconds)
        {
            PlayFootstep();
            timer = 0f;
        }
    }

    void PlayFootstep()
    {
        var audioClip = footstepClips[Random.Range(0, footstepClips.Length)];
        sfxPool.PlayPooledSource(audioClip, transform.position, 0.8f);
    }

    void OnRunningStateChanged(bool started)
    {
        isInRunningState = started;
    }
}