using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChordType
{
    None,
    MajorChord,
    MinorChord,
    PowerChord
}

public class GuitarController : MonoBehaviour
{
    [SerializeField] AllNotesContainer allNotesContainer;
    [SerializeField] AllScalesContainer allScalesContainer;
    [Tooltip("Determines the root of the scale. 0 = low E, 1 = F, 2 = F#")]
    public int rootIndexAllNotes = 0; // determines the root note on in allNotes (0=E, 1=F, 2=F#)
    [Tooltip("Determines what scale to use")]
    public ScaleType indexSelectedScale = ScaleType.Scale_I;

    // max rootNoteIndex should be 11
    
    public Note[] notesInKey = new Note[10]; // assignment of Notes depending on scale

    AudioSource audioSource;
    GuitarSpriteSelection guitarSpriteSelection;

    int activeNoteIndex = 1;
    ChordType activeChord = ChordType.None;
    bool sustainEnabled = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        guitarSpriteSelection = GetComponentInChildren<GuitarSpriteSelection>();
    }
    void Start()
    {
        AssignScale();
    }

    /// <summary>
    /// Turns note designation into an index
    /// Plays the note according to activeChordType
    /// </summary>
    public void EnterNoteInput(int note)
    {
        activeNoteIndex = note - 1;
        Play();
    }
    public void ApplyChordModifier(ChordType chordType)
    {
        activeChord = chordType;
    }
    void Play()
    {
        // notes have already been assigned to a scale
        // activeNoteIndex has already been assigned in EnterNoteInput
        switch (activeChord)
        {
            case ChordType.None:
                audioSource.clip = notesInKey[activeNoteIndex].pluck;
                break;
            case ChordType.MajorChord:
                audioSource.clip = notesInKey[activeNoteIndex].major;
                break;
            case ChordType.MinorChord:
                audioSource.clip = notesInKey[activeNoteIndex].minor;
                break;
            case ChordType.PowerChord:
                audioSource.clip = notesInKey[activeNoteIndex].power;
                break;
            default:
                Debug.LogWarning("No appropriate chord type entered for ChordType: " + activeChord);
                break;
        }

        print("playing: " + audioSource.clip.name);
        // sustain
        // Play() will be interrupted by next Play() while PlayOneShot() will not be interrupted
        if (sustainEnabled)
            audioSource.PlayOneShot(audioSource.clip);
        else
            audioSource.Play();

    }

    public void SetSustain(bool setValue)
    {
        sustainEnabled = setValue;
    }

    void AssignScale()
    {
        // assign first button (Alpha1) to root note
        int runningAllNotesIndex = rootIndexAllNotes;
        notesInKey[0] = allNotesContainer.allNotes[runningAllNotesIndex];

        // need to assign Note to all 9 remaining number keys according to spacings
        for (int i=1; i<10; i++)
        {
            // out of allNotes, increment index by scale spacing. Select scale using scaleIndex (0=major)
            // add runningAllNotesIndex to itself to cumulatively increment index
            runningAllNotesIndex = allScalesContainer.scales[(int)indexSelectedScale].spacings[i - 1] + runningAllNotesIndex;
            notesInKey[i] = allNotesContainer.allNotes[runningAllNotesIndex];
        }
    }

    public void ProcessGuitarSpriteCycleInput(bool forward)
    {
        guitarSpriteSelection.CycleGuitar(forward);
    }
}
