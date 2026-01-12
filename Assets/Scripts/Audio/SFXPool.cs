using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXPool : MonoBehaviour
{
    [SerializeField] int poolSize = 10;
    [SerializeField] AudioMixerGroup sfxMixer;

    Queue<PooledSource> availableSources = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            // create GameObject
            GameObject newPooledSFX = new GameObject($"PooledSFX_{i}");
            newPooledSFX.transform.parent = transform;

            // add an audio source
            AudioSource source = newPooledSFX.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixer;
            source.spatialBlend = 1f;
            source.playOnAwake = false;

            // add PooledSource component
            PooledSource pooled = newPooledSFX.AddComponent<PooledSource>();
            availableSources.Enqueue(pooled);
        }
    }

    /// <summary>
    /// grabs a new source from availableSources and plays clip with parameters
    /// Call this from external controller to use this class
    /// </summary>
    public void PlayPooledSource(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (availableSources.Count == 0)
            return; // or steal oldest source

        // grab PooledSource
        PooledSource pooledSource = availableSources.Dequeue();

        // when the clip is finished, use event to call ReturnToPool
        pooledSource.Play(clip, position, volume, ReturnToPool);
    }

    void ReturnToPool(PooledSource src)
    {
        availableSources.Enqueue(src);
    }
}