using UnityEngine;

public class PooledSource : MonoBehaviour
{
    // use of PooledSource avoids calling PlayOneShot, which creats a new AudioSource every call

    public AudioSource Source { get; private set; }
    private System.Action<PooledSource> onFinishedEvent;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    // can receive a method finishedCallback to perform at end of clip
    public void Play(AudioClip clip, Vector3 position, float volume, System.Action<PooledSource> finishedCallback)
    {
        transform.position = position;
        Source.clip = clip;
        Source.volume = volume;
        onFinishedEvent = finishedCallback;

        Source.Play();
        Invoke(nameof(Finish), clip.length / Source.pitch);
    }

    void Finish()
    {
        Source.Stop();
        onFinishedEvent?.Invoke(this);
    }
}