using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [HideInInspector]
    public bool isAvailableToTalk = true;   // determines whether or not the InteractableZone does stuff
    public bool watchPlayer = false;        // set to true to keep facing the player in Update
    public string displayName;              // used in dialogue box. Will default to Object.name
    public int manualDialogueIndex = 0;     // index of dialogue to be executed when the player deliberately initiates a conversation
    public int autoDialogueIndex = 0;       // index of dialogue to be executed when the player enters a zone that triggers a conversation
    public Dialogue[] allDialogues;         // array that can be filled out in GUI containing all Dialogues
    [HideInInspector]
    public int currentDialogueIndex;    // keeps track of what dialogue is playing so that it can be incremented

    bool faceRight = true;          // keeps track of Flip logic. Do not change outside of basic logic
    bool rightOfPlayer;             // keeps track of Flip logic. Do not change outside of basic logic

    // How to know when to activate the conversation
    private void Start()
    {
        if (displayName.Equals("")) { displayName = name; }  // sets displayName to name of GameObject by default // got da funky if statement formatting
        ResumeLife();
    }

    private void Update()
    {
        if (GameMaster.GM.thePlayer.transform.position.x < transform.position.x) // speaker is right of player
            rightOfPlayer = true;
        else // player is left of speaker
            rightOfPlayer = false;

        if (watchPlayer && ((rightOfPlayer && faceRight) || (!rightOfPlayer && !faceRight))) // if watchPlayer = true and facing the wrong way
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(Vector3.up* 180);
        faceRight = !faceRight;
    }

    public virtual void ResumeLife() // back to the grind
    {
        isAvailableToTalk = true;
    }

    public virtual void PlayerNearby()
    {
        // Display icon above Speaker
        BeginDialogue();
        ExecuteDialogue(autoDialogueIndex);
        // Dialogue to automatically play when player enters a zone
    }

    // =============================================== Dialogue stuff ==============================================
    public virtual void TriggerDialogueManually()
    {
        BeginDialogue();
        ExecuteDialogue(manualDialogueIndex);
    }

    public virtual void BeginDialogue()
    {
        isAvailableToTalk = false;
    }

    public virtual void ExecuteThisAction(string typedParameter) // using parameter typed in dialogue box, should be a name of a method
    {
        Invoke(typedParameter, 0f); // calls on next frame
    }

    public void ExecuteDialogue(int index)
    {
        // dialogueManager is a component of GameMaster Singleton
        GameMaster.GM.dialogueManager.StartDialogue(this, allDialogues[index]);
    }

    public void AnswerSelected(int indexNextDialogue)
    {
        currentDialogueIndex = indexNextDialogue;
        ExecuteDialogue(currentDialogueIndex);
    }
}
