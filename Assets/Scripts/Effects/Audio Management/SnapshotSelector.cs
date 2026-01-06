using UnityEngine;
using UnityEngine.Audio;

public class SnapshotSelector : MonoBehaviour
{
    [SerializeField] AudioMixerSnapshot defaultEvenSnapshot;
    [SerializeField] AudioMixerSnapshot guitarSnapshot;
    [SerializeField] float transitionSeconds = 0.3f;

    public void SwitchToDefault() => defaultEvenSnapshot.TransitionTo(transitionSeconds);
    public void SwitchToGuitar() => guitarSnapshot.TransitionTo(transitionSeconds);
}
