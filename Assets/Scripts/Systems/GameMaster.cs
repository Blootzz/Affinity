using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    // God
    public static GameMaster GM;

    // player reference if anyone else wants it
    public Player thePlayer;

    // Core stuff
    public bool paused = false;

    // Extra stuff
    public ControlManager controlManager = new ControlManager();
    public DialogueManager dialogueManager = new DialogueManager();
    public RespawnManager respawnManager = new RespawnManager();
    public List<GameObject> interactableTargets;
    public Canvas mainCanvas;

    // dialogue special command
    string dialogueParameter = ""; // the typed input that was typed into dialogue after \e, \c, etc. Cannot be null to use += operator


    void Awake()
    {
        if (GM != null)
            Destroy(GM);
        else
        {
            GM = this;

            Canvas[] canvases;
            canvases = FindObjectsOfType<Canvas>();
            foreach (Canvas x in canvases) // searches all canvases
                if (x.name.Equals("Canvas")) // If name is "Canvas"
                    mainCanvas = x;             // That is the main canvas
        }

        DontDestroyOnLoad(this);
    }// Create only 1 GameMaster

    private void Update()
    {
        if (Input.GetKeyDown(controlManager.pauseKey))
            PauseUnpause();
    }

    void PauseUnpause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            paused = true;
        }
    }

    //===================================================== ALL DIALOGUE STUFF =================================================
    // ---------------------------------------- BUTTON EVENTS -------------------------------------
    public void ContinueDialogue() // called by Dialogue UI system
    {
        if (dialogueManager.buttonInSelectPosition != null)
            dialogueManager.ClearSelectedButton();
        dialogueManager.AdvanceSentence();
        // calls dialogueManager.DisplayNextSentence()
        // DisplayNextSentence() performs checks for more sentences, question/answers, end of Dialogue
        // at end of Dialogue, currentSpeaker.NextDialogue is called
        // this either calls begins the next dialogue, or dialogueManager.EndDialogue()
    }

    public void SubmitAnswer(int selectedAnswerIndex) // 0 indexed (answer box one will return 0, answer box 2 will return 1)
    {
        dialogueManager.SelectAnswer(selectedAnswerIndex); // uses the index of the answer to access Answer currentDialogue.answers[]
    }// Used in OnClick() for answer button

    public void CloseConversation()
    {
        dialogueManager.FinishConversation();
    }

    // --------------------------- USING MONOBEHAVIOR ---------------------------
    public void DelayForAnswerSelect(GameObject thisButton)
    {
        StartCoroutine(dialogueManager.SelectButtonLater(thisButton));
    }

    public void DisplaySentence(string sentence)
    {
        StartCoroutine(DelayAndUpdate(sentence));
    }

    IEnumerator DelayAndUpdate(string sentence)
    {
        bool specialStuff = false; // used to make special commands not appear in text
        bool firstCharacterPassed = false; // determines whether or not the first letter AFTER '\' has been passed
        string commandMethod = "";
        bool doWait = false;

        dialogueManager.displayText.text = "";
        dialogueManager.continueButton.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            if (letter.Equals('\\'))
            {
                specialStuff = true;
            }// turn on listening for special

            if (specialStuff)
            {
                if (!firstCharacterPassed)
                {
                    if (!letter.Equals('\\')) // prevent '\' from being passed in as first character. If '\', nothing happens, but firstCharacterPassed remains false
                    {
                        firstCharacterPassed = true;
                        switch (char.ToLower(letter)) // input not case sensitive
                        {
                            case 'a':
                                commandMethod = "DialogueSpeakerAction"; //unique speaker action
                                break;

                            case 'c':
                                commandMethod = "DialogueChangeColor";
                                break;

                            case 'f':
                                commandMethod = "DialogueChangeFont";
                                break;

                            case 's':
                                commandMethod = "DialogueChangeSpeed";
                                break;

                            case 'w':
                                doWait = true;
                                break;
                        }// switch menu of special commands
                    }// prevent '\' from being passed in as first character
                }// if the first letter has not been passed yet
                else
                {
                    if (letter.Equals('\\')) // closing '\' is found
                    {
                        if (doWait)
                        {
                            yield return new WaitForSecondsRealtime(float.Parse(dialogueParameter));
                            doWait = false;
                        }// because waiting requires yield return new Wait... IN THIS COROUTINE, waiting has to be done here instead of in a method call
                        else
                        {
                            // execute commandMethod with parameter
                            ExecuteASpecialMethod(commandMethod); // carries out the method specified by the dialogue
                        }

                        // reset variables
                        yield return new WaitForEndOfFrame();
                        dialogueParameter = ""; // reset parameter
                        firstCharacterPassed = false;
                        specialStuff = false;
                    }
                    else
                        dialogueParameter += char.ToLower(letter).ToString(); // all parameters are lower case strings
                }// first letter has been passed
            }// special logic
            else // normal letter to be parsed
            {
                yield return new WaitForSeconds(dialogueManager.typeSpeed);  // VERY IMPORTANT THAT THIS IS FIRST
                dialogueManager.UpdateTextOneLetter(letter); // IDK WHY BUT THATS HOW IT IS
                if (letter.Equals('.') || letter.Equals(',') || letter.Equals('!') || letter.Equals('?')) // pause AFTER punctuation
                    yield return new WaitForSeconds(0.5f);
            }

        }// parsing entire sentence

        // End of sentence
        //yield return new WaitForSeconds(0.5f); // pause between sentences

        dialogueManager.WhatNext();


    }// MAIN DIALOGUE PARSE

    void ExecuteASpecialMethod(string commandMethod)
    {
        Invoke(commandMethod, 0f); // performs action on next frame
    }

    //--------------------------- DIALOGUE COMMANDS ---------------------------
    void DialogueSpeakerAction()
    {
        // <b><u><i> bold, underline, italics
        dialogueManager.SpeakerAction(dialogueParameter);
    }

    void DialogueChangeColor()
    {
        string color;
        // custom color assignment
        switch (dialogueParameter)
        {
            case "blue":
                color = dialogueManager.Blue;
                break;
            case "red":
                color = dialogueManager.Red;
                break;
            default:
                color = "#FFFFFF"; // white
                break;
        }
        if (color.Equals(""))
            Debug.LogWarning("new color set to \"\"");
        dialogueManager.displayText.text += "<color="+color+">"; // actually changes the color
    }// DialogueChangeColor

    void DialogueChangeFont()
    {
        string font;
        // custom color assignment
        switch (dialogueParameter)
        {
            //case "otherfont":
            //    font = OtherFont;
            //    break;
            default:
                font = dialogueManager.DefaultFont.name;
                break;
        }
        dialogueManager.displayText.text += "<font=" + font + ">"; // actually changes the color
    }

    void DialogueChangeSpeed()
    {
        switch (dialogueParameter)
        {
            case "default":
                dialogueManager.typeSpeed = dialogueManager.DefaultSpeed;
                break;
            case "slow":
                dialogueManager.typeSpeed = dialogueManager.SlowSpeed;
                break;
            case "fast":
                dialogueManager.typeSpeed = dialogueManager.FastSpeed;
                break;
            default:
                // read the string as a number
                dialogueManager.typeSpeed = float.Parse(dialogueParameter);
                break;
        }
    }

}// GameMaster