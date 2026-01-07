using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ChordType
{
    None,
    MajorChord,
    MinorChord,
    PowerChord
}

public class GuitarController : MonoBehaviour
{
    [SerializeField] GameObject GuitarUICanvas; // used to enable and disable with Hide Menu
    [SerializeField] AllNotesContainer allNotesContainer;
    [SerializeField] AllScalesContainer allScalesContainer;
    [Tooltip("Determines the root of the scale. 0 = low E, 1 = F, 2 = F#")]
    public int rootIndexAllNotes = 0; // determines the root note on in allNotes (0=E, 1=F, 2=F#)
    [Tooltip("Determines what scale to use")]
    public ScaleType indexSelectedScale = ScaleType.Scale_I;

    [Tooltip("If the player can play the 10th, this should be no greater than 1 octave + 2 whole steps from last index")]
    [SerializeField] int maxRootNoteIndex = 27;

    public Note[] notesInKey = new Note[10]; // assignment of Notes depending on scale

    AudioSource audioSource;
    SnapshotSelector snapshotSelector;
    GuitarSpriteSelection guitarSpriteSelection;
    StrummingArmSpriteSelection strumSpriteSelection;
    PitchShifter pitchShifter;

    int activeNoteIndex = 1;
    ChordType activeChord = ChordType.None;
    bool sustainEnabled = false;
    int sharpFlatIndexModifier = 0; // used to increment what note index to play depending on if sharp or flat is held down

    [Header("UI update events")]
    public UnityEvent<bool, bool> CycleHorizontalArrowInputEvent;
    public UnityEvent<bool, bool> CycleVerticalArrowInputEvent;
    public UnityEvent<int, bool> NoteInputEvent; // must be zero-indexed
    public UnityEvent<int, bool> ChordModifierInputEvent;
    public UnityEvent<int, int> AssignedScaleEvent; // passes index of root note and index of current scale
    public UnityEvent<bool, bool> PitchShiftEvent;
    public UnityEvent<bool, bool> SharpFlatEvent;
    public UnityEvent<bool> HideMenuEvent;

    // C# event for GuitarDetectionZone to listen to
    public event Action<int, ChordType> BroadcastNoteEvent;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        snapshotSelector = GetComponent<SnapshotSelector>();
        guitarSpriteSelection = GetComponentInChildren<GuitarSpriteSelection>();
        strumSpriteSelection = GetComponentInChildren<StrummingArmSpriteSelection>();
        pitchShifter = GetComponent<PitchShifter>();
    }

    private void OnEnable()
    {
        snapshotSelector.SwitchToGuitar();
        sharpFlatIndexModifier = 0;
    }
    private void OnDisable()
    {
        snapshotSelector.SwitchToDefault();
    }

    void Start()
    {
        AssignScale();
    }

    /// <summary>
    /// Turns note designation into an index
    /// Plays the note according to activeChordType
    /// </summary>
    public void EnterNoteInput(int note, bool buttonDown)
    {
        NoteInputEvent?.Invoke(note - 1, buttonDown);
        if (!buttonDown)
            return;

        activeNoteIndex = note - 1;
        strumSpriteSelection.Strum();
        BroadcastNoteEvent?.Invoke(note - 1, activeChord);
        Play();
    }
    public void ApplyChordModifier(ChordType chordType, bool buttonDown)
    {
        ChordModifierInputEvent?.Invoke((int)chordType, buttonDown);

        if (buttonDown)
        {
            activeChord = chordType;
            return;
        }
        // if receiving input same as active chord AND releasing the chord, clear chord modifier
        if (chordType.Equals(activeChord) && !buttonDown)
        {
            activeChord = ChordType.None;
            return;
        }
    }
    void Play()
    {
        Note noteToPlay = NoteWithSharpFlatModifier();
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

        if (audioSource.clip == null)
        {
            print("no clip found");
            return;
        }
        print("playing: " + audioSource.clip.name);
        // sustain
        // Play() will be interrupted by next Play() while PlayOneShot() will not be interrupted
        if (sustainEnabled)
            audioSource.PlayOneShot(audioSource.clip);
        else
            audioSource.Play();

    }

    void AssignScale()
    {
        // assign first button (Alpha1) to root note
        int runningAllNotesIndex = rootIndexAllNotes;
        notesInKey[0] = allNotesContainer.allNotes[runningAllNotesIndex];

        // need to assign Note to all 9 remaining number keys according to spacings
        for (int i = 1; i < notesInKey.Length; i++)
        {
            // out of allNotes, increment index by scale spacing. Select scale using scaleIndex (0=major)
            // add runningAllNotesIndex to itself to cumulatively increment index
            runningAllNotesIndex = allScalesContainer.scales[(int)indexSelectedScale].spacings[i - 1] + runningAllNotesIndex;
            notesInKey[i] = allNotesContainer.allNotes[runningAllNotesIndex];
        }

        AssignedScaleEvent?.Invoke(rootIndexAllNotes, (int)indexSelectedScale);
    }

    public void ProcessGuitarSpriteCycleInput(bool forward)
    {
        guitarSpriteSelection.CycleGuitar(forward);
    }


    public void ProcessCycleScaleInput(bool forward, bool buttonDown)
    {
        CycleHorizontalArrowInputEvent?.Invoke(forward, buttonDown);
        DoCycleScaleLogic(forward, buttonDown);
    }

    public void BUTTON_CycleScaleForward() => DoCycleScaleLogic(true, true);
    public void BUTTON_CycleScaleBackward() => DoCycleScaleLogic(false, true);

    public void DoCycleScaleLogic(bool forward, bool buttonDown)
    {
        if (!buttonDown)
            return;

        indexSelectedScale = (ScaleType)((int)indexSelectedScale + (forward ? 1 : -1));

        // check out of bounds
        if ((int)indexSelectedScale < 0)
            indexSelectedScale = (ScaleType)(allScalesContainer.scales.Length - 1);
        if ((int)indexSelectedScale > allScalesContainer.scales.Length - 1)
            indexSelectedScale = 0;

        // double check
        if (!Enum.IsDefined(typeof(ScaleType), (int)indexSelectedScale))
            Debug.LogWarning("Invalid ScaleType selected: " + (int)indexSelectedScale);

        AssignScale();
    }


    /// <summary>
    /// call from input action. Fires event listened to by UI to update button visuals but EVENT DOES NOT DO LOGIC
    /// </summary>
    public void ProcessCycleKeyInput(bool forward, bool buttonDown)
    {
        CycleVerticalArrowInputEvent?.Invoke(forward, buttonDown);
        DoCycleKeyLogic(forward, buttonDown);
    }

    /// called by UI button
    /// assign to whatever clickable button should advance the key up
    public void BUTTON_CycleKeyForward() => DoCycleKeyLogic(true, true);
    public void BUTTON_CycleKeyBackward() => DoCycleKeyLogic(false, true);

    void DoCycleKeyLogic(bool forward, bool buttonDown)
    {
        if (!buttonDown)
            return;

        rootIndexAllNotes += forward ? 1 : -1;

        if (rootIndexAllNotes < 0)
            rootIndexAllNotes = maxRootNoteIndex;
        if (rootIndexAllNotes > maxRootNoteIndex)
            rootIndexAllNotes = 0;

        AssignScale();
    }

    public void SetSustain(bool setValue)
    {
        sustainEnabled = setValue;
    }

    public void ProcessBend(bool useHalfStep, bool buttonDown)
    {
        PitchShiftEvent?.Invoke(useHalfStep, buttonDown);
        DoBendLogic(useHalfStep, buttonDown);
    }
    void DoBendLogic(bool useHalfStep, bool buttonDown)
    {
        pitchShifter.PitchShift(useHalfStep, buttonDown);
    }

    public void ProcessSharpFlat(bool useSharp, bool buttonDown)
    {
        SharpFlatEvent?.Invoke(useSharp, buttonDown);
        DoSharpFlat(useSharp, buttonDown);
    }
    void DoSharpFlat(bool useSharp, bool buttonDown)
    {
        if (buttonDown)
        {
            sharpFlatIndexModifier = useSharp ? 1 : -1;
            return;
        }

        // ignore button up input if it wasn't the last sharp/flat button pressed down
        // If this input is the same as the last button, set modifier to 0
        int input = useSharp ? 1 : -1;
        if (input == sharpFlatIndexModifier)
            sharpFlatIndexModifier = 0;

    }
    // called when a new index is selected in EnterNoteInput
    Note NoteWithSharpFlatModifier()
    {
        int i; // all notes index
        for (i = 0; i < allNotesContainer.allNotes.Length - 1; i++)
        {
            if (allNotesContainer.allNotes[i].Equals(notesInKey[activeNoteIndex]))
                break;
        }
        
        // apply Sharp or flat adder/subtractor
        i += sharpFlatIndexModifier;

        // correct out of bounds error by resetting it back to what it was before modifier
        if (i == -1)
            i = 0;
        if (i == allNotesContainer.allNotes.Length)
            i = allNotesContainer.allNotes.Length - 1;

        return allNotesContainer.allNotes[i];
    }

    public void ProcessToggleHideMenu(bool buttonDown)
    {
        HideMenuEvent?.Invoke(buttonDown);
        DoToggleHideMenuLogic(buttonDown);
    }
    void DoToggleHideMenuLogic(bool buttonDown)
    {
        // only execute when buttonDown == true
        if (!buttonDown)
            return;

        if (GuitarUICanvas.activeInHierarchy)
            GuitarUICanvas.SetActive(false);
        else
            GuitarUICanvas.SetActive(true);
    }
}
