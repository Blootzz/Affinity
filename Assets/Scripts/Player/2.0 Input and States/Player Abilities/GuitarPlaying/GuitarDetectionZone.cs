using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GuitarDetectionZone : MonoBehaviour
{
    GuitarController guitarController;
    [SerializeField] RecordableNote[] answerKey;
    [SerializeField] RecordableNote[] notesRecorded;

    public UnityEvent SuccessfulNotesEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out guitarController))
        {
            guitarController.BroadcastNoteEvent += RecordNote;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<GuitarController>())
        {
            // prevents exit play error. Exiting play still runs code below, possibly due to being called twice?
            if (guitarController == null)
                return;

            guitarController.BroadcastNoteEvent -= RecordNote;
            guitarController = null;
         }
    }

    private void Start()
    {
        if (notesRecorded.Length != answerKey.Length)
            Debug.LogWarning("notesRecorded and answerKey do not have the same array lengths | notesRecorded.Length: " + notesRecorded.Length + " | answerKey.Length: " + answerKey.Length);
    }

    /// <summary>
    /// Assumes notesRecorded starts with default values that are not null
    /// Pushes in new value from right side of list
    /// Compares to answerKey
    /// </summary>
    void RecordNote(int noteIndex, ChordType chordType)
    {
        print("recording note: " + noteIndex + " | chordType: " + chordType);
        // shift all values to the left and add this at the end
        // stop short of final index to avoid i+1 out of bounds error. [Length-1] will be filled in after for loop
        for (int i = 0; i <= notesRecorded.Length - 2; i++)
            notesRecorded[i] = notesRecorded[i + 1];
        notesRecorded[^1] = new RecordableNote(noteIndex, chordType); // ^1 = Length-1

        if (EvaluateNoteSequence())
        {
            print("Successful comparison between answerKey and notesRecorded");
            SuccessfulNotesEvent?.Invoke();
        }
    }

    // if one RecordableNote does note match, returns false
    bool EvaluateNoteSequence()
    {
        for (int i = 0; i < answerKey.Length; i++)
        {
            if (!answerKey[i].Equals(notesRecorded[i]))
                return false;
        }
        return true;
    }

}

[System.Serializable]
public struct RecordableNote
{
    public int allNoteIndex;
    public ChordType chordType;

    public RecordableNote(int index, ChordType chord)
    {
        allNoteIndex = index;
        chordType = chord;
    }

}