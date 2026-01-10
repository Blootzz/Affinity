using UnityEngine;

public class DuoSourceManager : MonoBehaviour
{
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    VolumeController volumeController1;
    VolumeController volumeController2;
    PitchShifter pitchShifter1;
    PitchShifter pitchShifter2;

    bool source1Free = true;

    private void Awake()
    {
        volumeController1 = source1.GetComponent<VolumeController>();
        volumeController2 = source2.GetComponent<VolumeController>();
        pitchShifter1 = source1.GetComponent<PitchShifter>();
        pitchShifter2 = source2.GetComponent<PitchShifter>();
    }

    public void PlayClipSmart(AudioClip clipToPlay)
    {
        if (source1Free)
        {
            source1.clip = clipToPlay;
            source1.Play();
            volumeController1.ResetVolume();
            volumeController2.FadeOut();
        }
        else
        {
            source2.clip = clipToPlay;
            source2.Play();
            volumeController2.ResetVolume();
            volumeController1.FadeOut();
        }

        source1Free = !source1Free;
    }

    public void FadeBoth()
    {
        volumeController1.FadeOut();
        volumeController2.FadeOut();
    }
    public void PitchShiftSmart(bool useHalfStep, bool buttonDown)
    {
        if (!source1Free) // source is most recently used
            pitchShifter1.PitchShift(useHalfStep, buttonDown);
        else
            pitchShifter2.PitchShift(useHalfStep, buttonDown);
    }
}
