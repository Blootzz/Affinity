using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Dialogue
{
    [TextArea(3,10)]                // minimum of 3 lines in inspector
    public string[] sentences;      // text to be said before player input. This will either end with a question or close the conversation with the player
    public int nextDialogueIndex = -1;   // directs to next dialogue. If there are no answers and nextDialogueIndex == -1, the conversation closes
    public Answer[] answers;        // list of answer choices. If a dialogue's Answer[] array has length of 0, the DialogueManager should close the conversation
}

[System.Serializable] // I spent so long trying to get this serialized and not just a blank reference. Turns out I just had to not give it its own script
public class Answer
{
    public int poiseRequirement;     // requirement for this option to be available. Compare to int Player.poise
    public string text;              // text of answer choice
    public int indexDialogue;        // if selected, DialogueManager will send this number to Speaker. Speaker will select Dialogue[indexDialogue]
    public bool makeDefault = false; // if true, this answer choice will be sure to be highlighted first
}