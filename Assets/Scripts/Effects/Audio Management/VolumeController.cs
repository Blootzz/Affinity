using UnityEngine;
using System.Collections;

public class VolumeController : MonoBehaviour
{
    [SerializeField] float fadeSeconds = 0.2f;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeOut()
    {
        StartCoroutine(TaperVolume());
    }

    IEnumerator TaperVolume()
    {
        float numFixedUpdates = fadeSeconds / Time.fixedDeltaTime;
        for (int iterations = 0; iterations < numFixedUpdates; iterations++)
        {
            audioSource.volume = 1 - (iterations / numFixedUpdates);
            yield return new WaitForSecondsRealtime(fadeSeconds / numFixedUpdates);
        }
    }

    public void ResetVolume()
    {
        StopAllCoroutines();
        audioSource.volume = 1;
    }
}
