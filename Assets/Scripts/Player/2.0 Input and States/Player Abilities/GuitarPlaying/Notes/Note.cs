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

[CreateAssetMenu(menuName = "Guitar/All Notes Container")]
public class AllNotesContainer : ScriptableObject
{
    public Note[] allNotes; // storage of all Note data
}