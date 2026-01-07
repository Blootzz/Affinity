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
    float halfStepPitchAdder = 1 / 12f;
    float wholeStepPitchAdder = 2 / 12f;

    public void PitchShift(bool up, bool useHalfStep)
    {
        StopAllCoroutines();
        audioSource.pitch = 1;
        StartCoroutine(ShiftPitchPerFixedUpdate(up, useHalfStep));
    }

    IEnumerator ShiftPitchPerFixedUpdate(bool bendUp, bool useHalfStep)
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
