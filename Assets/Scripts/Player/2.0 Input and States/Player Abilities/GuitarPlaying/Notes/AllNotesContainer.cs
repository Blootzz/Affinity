using UnityEngine;

[CreateAssetMenu(menuName = "Guitar/All Notes Container")]
public class AllNotesContainer : ScriptableObject
{
    public Note[] allNotes; // storage of all Note data
}