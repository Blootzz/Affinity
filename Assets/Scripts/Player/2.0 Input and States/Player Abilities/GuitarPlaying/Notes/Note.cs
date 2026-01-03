using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName ="Guitar/Single Note")]
public class Note : ScriptableObject
{
    public AudioClip pluck;
    public AudioClip major;
    public AudioClip minor;
    public AudioClip power;
}

