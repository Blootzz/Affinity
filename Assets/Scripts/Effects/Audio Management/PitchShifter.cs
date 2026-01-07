using UnityEngine;
using System.Collections;

public class PitchShifter : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [Tooltip("Normalized graph to be added to pitch every FixedUpdate")]
    [SerializeField] AnimationCurve bendPitchGraph;

    public bool useFastBend = false;
    [SerializeField] float fastBendSeconds = 0.2f;
    [SerializeField] float slowBendSeconds = 0.6f;
    readonly float halfStepPitchAdder = 1 / 12f;
    readonly float wholeStepPitchAdder = 2 / 12f;

    public void PitchShift(bool useHalfStep, bool pitchUp)
    {
        StopAllCoroutines();
        audioSource.pitch = 1;
        StartCoroutine(ShiftPitchPerFixedUpdate(useHalfStep, pitchUp));
    }

    IEnumerator ShiftPitchPerFixedUpdate(bool useHalfStep, bool bendUp)
    {
        float numFixedUpdates = (useFastBend ? fastBendSeconds : slowBendSeconds) / Time.fixedDeltaTime;
        float totalBendPitchAdder = useHalfStep ? halfStepPitchAdder : wholeStepPitchAdder;
        
        // bend up -> traverse forwards
        if (bendUp)
        {
            for (float bendX = 0; bendX <= 1; bendX += 1/numFixedUpdates)
            {
                audioSource.pitch = 1 + bendPitchGraph.Evaluate(bendX) * totalBendPitchAdder;
                yield return new WaitForFixedUpdate();
            }
        }
        else // bend down -> traverse backwards
        {
            for (float bendX = 1; bendX >= 0; bendX -= 1 / numFixedUpdates)
            {
                audioSource.pitch = 1 + bendPitchGraph.Evaluate(bendX) * totalBendPitchAdder;
                yield return new WaitForFixedUpdate();
            }
        }

    }
}
