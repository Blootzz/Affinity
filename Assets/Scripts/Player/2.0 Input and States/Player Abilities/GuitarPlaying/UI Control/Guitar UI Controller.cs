using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Header("Note Illumination")]
    [SerializeField] Color32 lightBlue;
    [SerializeField] Color32 darkBlue;
    [Tooltip("Parent containing all the notes. All notes contain another child (the illuminated number)")]
    [SerializeField] GameObject NoteMarkersParent;

    [Header("Selector Square Positioning Data")]
    [SerializeField] RectTransform SelectorSquareRectTransform;
    [Tooltip("Distance from far left of panel just to get on to first scale option")]
    [SerializeField] float baseSelectorOffset = 6;
    [Header("Position difference between each scale on panel")]
    [SerializeField] float selectorDistancePerScale = 34;

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
    public void LISTEN_OnAssignedScale(int scaleIndex)
    {
        float newXPosition = baseSelectorOffset + selectorDistancePerScale * scaleIndex;
        SelectorSquareRectTransform.anchoredPosition = new Vector2(newXPosition, 0);
    }
}
