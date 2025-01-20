using UnityEngine;
using UnityEngine.UI;

public class AnswerButtonScript : MonoBehaviour
{
    // UI canvas is in sortingOrder = 1
    // Both of these are called in the animator
    // _MoveBack() in Down
    // _MoveFront in Selected
    public void _MoveBack()
    {
        GetComponentInParent<Canvas>().sortingOrder = 0;
    }

    public void _MoveFront()
    {
        GetComponentInParent<Canvas>().sortingOrder = 2;        
    }

    public void _MakeInteractableFalse()
    {
        GetComponent<Button>().interactable = false;
        // for some reason, trying to change this property in the animator dopesheet caused this to be called prematurely regardless of if Out1, Out2, etc. is being called
        // interactable is set to true in DialogueManager
    }
    
}
