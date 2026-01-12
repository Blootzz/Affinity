using UnityEngine;
using System.Collections;
using System;

public class PitchShifter : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [Tooltip("Normalized graph to be added to pitch every FixedUpdate")]
    [SerializeField] AnimationCurve bendPitchGraph;

    public bool useFastBend = false;
    [SerializeField] float fastBendSeconds = 0.2f;
    [SerializeField] float slowBendSeconds = 0.6f;
    readonly float halfStepPitchAdder = 0.059463f;
    readonly float wholeStepPitchAdder = 0.122462f;

    //bool shiftActive = false; // tracker so OnPlayNextNote knows how to handle note input
    bool isBentUp = false; // tracker to help cancel pitch shift so negative input is nullified
    //int noteLastPlayedIndex;

    public void PitchShift(bool useHalfStep, bool pitchUp)
    {
        ResetAll();
        StartCoroutine(ShiftPitchPerFixedUpdate(useHalfStep, pitchUp));
    }

    IEnumerator ShiftPitchPerFixedUpdate(bool useHalfStep, bool bendUp)
    {
        //shiftActive = true;

        float numFixedUpdates = (useFastBend ? fastBendSeconds : slowBendSeconds) / Time.fixedDeltaTime;
        float totalBendPitchAdder = useHalfStep ? halfStepPitchAdder : wholeStepPitchAdder;
        
        // bend up -> traverse forwards
        if (bendUp)
        {
            isBentUp = true;

            for (float bendX = 0; bendX <= 1; bendX += 1/numFixedUpdates)
            {
                audioSource.pitch = 1 + bendPitchGraph.Evaluate(bendX) * totalBendPitchAdder;
                yield return new WaitForFixedUpdate();
            }
        }
        else if(isBentUp) // bend down -> traverse loop backwards
        {
            
            for (float bendX = 1; bendX >= 0; bendX -= 1 / numFixedUpdates)
            {
                audioSource.pitch = 1 + bendPitchGraph.Evaluate(bendX) * totalBendPitchAdder;
                yield return new WaitForFixedUpdate();
            }
        }

        //shiftActive = false;
    }

    public void OnPlayNextNote(int noteIndex, bool buttonDown)
    {
        //noteLastPlayedIndex = noteIndex;

        if (buttonDown)
        {

        }

        if (!buttonDown)
        {

        }
            
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        audioSource.pitch = 1;
    }

    /// <summary>
    /// Used to allow note 1 to be bent then note 2 is immediately bent and can be released
    /// </summary>
    public void SnapToBend(bool useHalfBend)
    {
        audioSource.pitch = 1 + (useHalfBend ? halfStepPitchAdder : wholeStepPitchAdder);
    }
}
