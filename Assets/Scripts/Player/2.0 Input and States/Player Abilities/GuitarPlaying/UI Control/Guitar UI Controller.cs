using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// use Unity Events in GuitarController to subscribe these methods in editor
// As long as no custom types are involved, no asemdef reference should be required
public class GuitarUIController : MonoBehaviour
{
    [Header("Arrow Button Visuals")]
    [SerializeField] Button UpArrow;
    [SerializeField] Button DownArrow;
    [SerializeField] Button RightArrow;
    [SerializeField] Button LeftArrow;

    [Header("Chord Modifier Button Visuals")]
    [SerializeField] Button MajorChordButton;
    [SerializeField] Button MinorChordButton;
    [SerializeField] Button PowerChordButton;

    [Header("Bend modifiers")]
    [SerializeField] Button HalfBendButton;
    [SerializeField] Button WholeBendButton;

    [Header("Sharp/Flat Buttons")]
    [SerializeField] Button SharpButton;
    [SerializeField] Button FlatButton;

    [Header("Other Buttons")]
    [SerializeField] Button HideMenuButton;

    [Header("Selector Square Positioning Data")]
    [SerializeField] RectTransform SelectorSquareRectTransform;
    [Tooltip("Distance from far left of panel just to get on to first scale option")]
    [SerializeField] float baseSelectorOffset = 6;
    [Header("Position difference between each scale on panel")]
    [SerializeField] float selectorDistancePerScale = 34;

    [Header("Note Marker Positioning and Illumination")]
    [SerializeField] float distanceWholeStep;
    [SerializeField] float distanceHalfStep;

    [SerializeField] Color32 lightBlue;
    [SerializeField] Color32 darkBlue;
    [Tooltip("Parent containing all the notes. All notes contain another child (the illuminated number)")]
    [SerializeField] GameObject NoteMarkersParent;
    [SerializeField] RectTransform horizontalRail;

    [Header("Note and scale container reference")]
    [SerializeField] AllScalesContainer allScalesContainer;
    [SerializeField] AllNotesContainer allNotesContainer;

    [Header("Text fields to update on scale change")]
    [SerializeField] TextMeshProUGUI rootText;
    [SerializeField] TextMeshProUGUI scaleText;


    /// <summary>
    /// Needs to be triggered by GuitarController
    /// Does not do button.onClick() logic. Let the button lead straight to GuitarController.BUTTON_CycleKeyForward()
    /// </summary>
    public void LISTEN_OnVerticalArrowButtonPressed(bool usingUpArrow, bool buttonDown)
    {
        Button arrowButton;
        if (usingUpArrow)
            arrowButton = UpArrow;
        else
            arrowButton = DownArrow;

        if (buttonDown)
            arrowButton.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            arrowButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }

    public void LISTEN_OnHorizontalArrowButtonPressed(bool usingRightArrow, bool buttonDown)
    {
        Button arrowButton;
        if (usingRightArrow)
            arrowButton = RightArrow;
        else
            arrowButton = LeftArrow;

        if (buttonDown)
            arrowButton.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            arrowButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }

    public void LISTEN_OnChordButtonPressed(int indexChordType, bool buttonDown)
    {
        Button chordButtonToChange;

        ChordType chordType = (ChordType)indexChordType;
        switch (chordType)
        {
            case ChordType.None:
                Debug.LogError("How tf did ChordType.None get here? This should just be for processing input actions for chord modifiers");
                return; // avoids null ref
            case ChordType.MajorChord:
                chordButtonToChange = MajorChordButton;
                break;
            case ChordType.MinorChord:
                chordButtonToChange = MinorChordButton;
                break;
            case ChordType.PowerChord:
                chordButtonToChange = PowerChordButton;
                break;
            default:
                Debug.LogError("Invalid chord type to change button sprite");
                return; // avoids null ref
        }

        if (buttonDown)
            chordButtonToChange.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            chordButtonToChange.OnPointerUp(new PointerEventData(EventSystem.current));
    }

    /// <summary>
    /// must be zero-indexed. Will probably behave incorrectly if notes before root are added
    /// notePlayedIndex is the child index of the note in Object "Note Markers".
    /// The illuminated number is the child of that child
    /// </summary>
    public void LISTEN_IlluminateOnNotePlayed(int notePlayedIndex, bool buttonDown)
    {
        Color32 updatedColor;
        if (buttonDown)
            updatedColor = lightBlue;
        else
            updatedColor = darkBlue;

        Image selectedImage = NoteMarkersParent.transform.GetChild(notePlayedIndex). // get correct note by index
            transform.GetChild(0).GetComponent<Image>(); // image is first child of note

        selectedImage.color = updatedColor;
    }

    /// <summary>
    /// sets selector square to target scale based on index
    /// </summary>
    public void LISTEN_OnAssignedScale(int rootNoteIndex, int scaleIndex)
    {
        MoveSelectorSquare(scaleIndex);
        MoveNotes(scaleIndex);
        UpdateLabels(rootNoteIndex, scaleIndex);
    }
    void MoveSelectorSquare(int scaleIndex)
    {
        float newXPosition = baseSelectorOffset + selectorDistancePerScale * scaleIndex;
        SelectorSquareRectTransform.anchoredPosition = new Vector2(newXPosition, 0);
    }
    /// <summary>
    /// synchronizes each note marker to be positioned by the selected scale steps
    /// </summary>
    void MoveNotes(int selectedScaleIndex)
    {
        // iterate through every note marker
        foreach (RectTransform childMarker in NoteMarkersParent.transform)
        {
            float cumulativeDistanceFromFarLeft = 0;
            // iterate through scale spacing up to this marker's index
            // start on 1 because first note will always be on root, avoid index out of bounds error in spacings[]
            for (int noteIndex = 1; noteIndex <= childMarker.GetSiblingIndex(); noteIndex++)
            {
                float distanceToAdd = 0;
                int stepNum = allScalesContainer.scales[selectedScaleIndex].spacings[noteIndex-1];
                if (stepNum == 1)
                    distanceToAdd = distanceHalfStep;
                else if (stepNum == 2)
                    distanceToAdd = distanceWholeStep;
                else
                    Debug.LogError("indexed stepNum is not a 1 or 2: " + stepNum);

                cumulativeDistanceFromFarLeft += distanceToAdd;
            }

            childMarker.anchoredPosition = new Vector2(cumulativeDistanceFromFarLeft, 0);

            // resize horizontal rail every interation, allowing the last iteration for the last note assign the "height"
            horizontalRail.sizeDelta = new Vector2(4, cumulativeDistanceFromFarLeft);
        }
    }
    void UpdateLabels(int rootNoteIndex, int scaleIndex)
    {
        rootText.text = allNotesContainer.allNotes[rootNoteIndex].ToString();
        scaleText.text = allScalesContainer.scales[scaleIndex].nameDesignation;
    }

    public void LISTEN_OnBendPitch(bool useHalfStep, bool buttonDown)
    {
        Button bendButton;
        if (useHalfStep)
            bendButton = HalfBendButton;
        else
            bendButton = WholeBendButton;

        if (buttonDown)
            bendButton.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            bendButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }
    public void LISTEN_OnSharpFlat(bool useSharp, bool buttonDown)
    {
        Button sharpOrFlatButton;
        if (useSharp)
            sharpOrFlatButton = SharpButton;
        else
            sharpOrFlatButton = FlatButton;

        if (buttonDown)
            sharpOrFlatButton.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            sharpOrFlatButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }

    public void LISTEN_OnHideMenu(bool buttonDown)
    {
        if (buttonDown)
            HideMenuButton.OnPointerDown(new PointerEventData(EventSystem.current));
        else
            HideMenuButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }
}
