using UnityEngine;

public class DuoSourceManager : MonoBehaviour
{
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    VolumeController volumeController1;
    VolumeController volumeController2;
    PitchShifter pitchShifter1;
    PitchShifter pitchShifter2;

    bool playSource1Next = true;

    bool isBendActive = false; // if note 1 is bent and not released, note 2 should also be bent
    bool usingHalfStep; // used to track most recent bend type

    private void Awake()
    {
        volumeController1 = source1.GetComponent<VolumeController>();
        volumeController2 = source2.GetComponent<VolumeController>();
        pitchShifter1 = source1.GetComponent<PitchShifter>();
        pitchShifter2 = source2.GetComponent<PitchShifter>();
    }

    public void PlayClipSmart(AudioClip clipToPlay)
    {
        if (playSource1Next)
        {
            if (isBendActive)
                pitchShifter1.SnapToBend(usingHalfStep);
            
            source1.clip = clipToPlay;
            source1.Play();
            volumeController1.ResetVolume();

            volumeController2.FadeOut();
            pitchShifter2.ResetAll();
        }
        else
        {
            if (isBendActive)
                pitchShifter2.SnapToBend(usingHalfStep);
            source2.clip = clipToPlay;
            source2.Play();
            volumeController2.ResetVolume();

            volumeController1.FadeOut();
            pitchShifter1.ResetAll();
        }

        playSource1Next = !playSource1Next;
    }

    public void FadeBoth()
    {
        volumeController1.FadeOut();
        volumeController2.FadeOut();
    }
    public void PitchShiftSmart(bool useHalfStep, bool buttonDown)
    {
        if (buttonDown)
            isBendActive = true;
        else
            isBendActive = false;

        if (!playSource1Next) // source is most recently used
            pitchShifter1.PitchShift(useHalfStep, buttonDown);
        else
            pitchShifter2.PitchShift(useHalfStep, buttonDown);
    }
}
