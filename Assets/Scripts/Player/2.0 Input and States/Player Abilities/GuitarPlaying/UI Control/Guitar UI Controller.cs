using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// use Unity Events in GuitarController to subscribe these methods in editor
// As long as no custom types are involved, no asemdef reference should be required
public class GuitarUIController : MonoBehaviour
{
    [SerializeField] Button UpArrow;
    [SerializeField] Button DownArrow;
    [SerializeField] Button RightArrow;
    [SerializeField] Button LeftArrow;

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
        {
            arrowButton.OnPointerDown(new PointerEventData(EventSystem.current));
        }
        else
            arrowButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }

    public void LISTEN_OnHorizontalArrowButtonPressed(bool usingRightArrow, bool buttonDown)
    {
        Button arrowButton;
        if (usingRightArrow)
            arrowButton = RightArrow;
        else
            arrowButton = RightArrow;

        if (buttonDown)
        {
            arrowButton.OnPointerDown(new PointerEventData(EventSystem.current));
        }
        else
            arrowButton.OnPointerUp(new PointerEventData(EventSystem.current));
    }
}
