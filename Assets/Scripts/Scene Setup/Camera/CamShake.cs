using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CamShake : MonoBehaviour
{
    CinemachineBasicMultiChannelPerlin cineNoiseController;
    [SerializeField] float baseAmplitude = 3;
    [Tooltip("Determines how incoming damage scales with screenshake. 1 = no extra scaling")]
    [SerializeField] float damageMultiplierWeight = 1;
    [SerializeField] float baseFrequency = 0.1f;
    [Tooltip("Seconds until shake is completely faded out")]
    [SerializeField] float baseTaperSeconds = 0.5f;
    [Tooltip("Number of times FadeOutShake will iterate during taperSeconds")]
    [SerializeField] int fadeResolution = 4;

    float startingAmplitudeWeightedByDamage; // used privately to store result of baseAmplitude * damage * damageMultiplierWeight
    float taperSecondsWeightedByDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cineNoiseController = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// Shakes camera by multiplying <paramref name="damage"/> by damageMultiplierWeight and baseAmplitude
    /// </summary>
    /// <param name="damageMultiplier"></param>
    public void BeginCameraShake(float damage)
    {
        taperSecondsWeightedByDamage = baseTaperSeconds * damage / 20 * damageMultiplierWeight;
        startingAmplitudeWeightedByDamage = baseAmplitude * damage / 20 * damageMultiplierWeight; // using 20 damage as reference for one "unit" of shake
        cineNoiseController.AmplitudeGain = startingAmplitudeWeightedByDamage;
        cineNoiseController.FrequencyGain = baseFrequency;
        StartCoroutine(FadeOutShake());
    }
    IEnumerator FadeOutShake()
    {
        for (int i = 0; i < fadeResolution; i++)
        {
            yield return new WaitForSecondsRealtime(taperSecondsWeightedByDamage / fadeResolution);
            cineNoiseController.AmplitudeGain -= startingAmplitudeWeightedByDamage / fadeResolution; // subtract fraction of baseAmplitude
            cineNoiseController.FrequencyGain -= baseFrequency / fadeResolution; // subtract fraction of baseFrequency
        }
    }
}
