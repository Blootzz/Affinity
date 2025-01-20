using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System;

[System.Serializable]
public class DialogueManager
{
    public Player thePlayer;
    Speaker currentSpeaker;
    Dialogue currentDialogue;
    [HideInInspector]
    public Animator barsAnimator;
    [HideInInspector]
    public Animator nameAnimator;
    [HideInInspector]
    public Animator answersAnimator;
    [HideInInspector]
    public Animator HUDAnimator;

    [HideInInspector]
    public TextMeshProUGUI displayText;
    [HideInInspector]
    public TextMeshProUGUI nameText;
    [HideInInspector]
    public TextMeshProUGUI answer1Text;
    [HideInInspector]
    public TextMeshProUGUI answer2Text;
    [HideInInspector]
    public TextMeshProUGUI answer3Text;
    [HideInInspector]
    public TextMeshProUGUI answer4Text;
    [HideInInspector]
    public TextMeshProUGUI answer5Text;
    GameObject[] answerButtons = new GameObject[5];
    [HideInInspector]
    public GameObject answer1;
    [HideInInspector]
    public GameObject answer2;
    [HideInInspector]
    public GameObject answer3;
    [HideInInspector]
    public GameObject answer4;
    [HideInInspector]
    public GameObject answer5;

    public GameObject continueButton;
    public GameObject closeButton;

    [HideInInspector]
    public GameObject buttonInSelectPosition = null;
    [HideInInspector]
    public int sentenceIndex;

    // GameMaster variables
    // hex colors
    public string Blue = "#80D4FF";
    public string Red = "#e33434";
    // fonts
    [HideInInspector]
    public Font DefaultFont;
    // speeds (lower=faster)
    [HideInInspector]
    public float typeSpeed = 0.03f;
    public readonly float DefaultSpeed = 0.03f;
    public readonly float SlowSpeed = 0.1f;
    public readonly float FastSpeed = 0.01f;

    public void Startup()
    {
        closeButton.SetActive(false);
        //closeButton.GetComponent<Button>().enabled = false;
        answerButtons[0] = answer1;
        answerButtons[1] = answer2;
        answerButtons[2] = answer3;
        answerButtons[3] = answer4;
        answerButtons[4] = answer5;
        sentenceIndex = -1; // sentenceIndex will be increased by 1 before being used
        displayText.text = null;
    }

    public void StartDialogue(Speaker speaker, Dialogue dialogue) // called on continuation of a conversation, not just the start
    {
        thePlayer.controlsDisabled = true;
        HUDAnimator.SetBool("Raised", true); // for some reason, using a trigger system instead of bool causes extra calls to raise up when dialogue finishes
        barsAnimator.SetBool("isOpen", true);
        nameAnimator.SetBool("isUp", true);
        currentSpeaker = speaker;
        currentDialogue = dialogue;
        nameText.text = currentSpeaker.displayName;
        Startup();
        //DisableAnswers();
        continueButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null); // apparently this is a necessary thing to do in order to actually set it to \
        AdvanceSentence();
    }

    public void WhatNext()
    {
        if (sentenceIndex + 1 < currentDialogue.sentences.Length)
        {
            TurnOnContinueButton(); // will lead to AdvanceSentence()
        }// if there are still more sentences in the dialogue
        else
        {
            // last sentence in dialogue has been displayed
            if (currentDialogue.answers.Length > 0)        // if there are any answers to give
                DisplayAnswers();   // Execute answer choice display
            else
            {
                // end convo
                EndDialogue();
            }// no question
        }// no more sentences
    }

    public bool CheckAnswersAvailable()
    {
        if (currentDialogue.answers.Length > 0)        // if there are any answers to give
            return true;
        return false;
    }

    public bool CheckMoreSentencesLeft()
    {
        if (sentenceIndex + 1 < currentDialogue.sentences.Length) // includes +1 because AdvanceSentence() hasn't increased the index yet
            return true;
        return false;
    }

    public void UpdateTextOneLetter(char letter)
    {
        displayText.text += letter;
    }

    public void AdvanceSentence()
    {
        sentenceIndex++;
        GameMaster.GM.DisplaySentence(currentDialogue.sentences[sentenceIndex]);
        // uses GameMaster's monobehavior to start Coroutine in GameMaster
        // delays each letter by typeSpeed
    }


    public void DisplayAnswers()
    {
        bool defaultSet = false;
        for (int i = 0; i < currentDialogue.answers.Length; i++)
        {
            answerButtons[i].SetActive(true);
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.answers[i].text;
            answerButtons[i].GetComponent<Animator>().SetTrigger("ListAnswer");
            answerButtons[i].GetComponent<Button>().interactable = true;

            if (currentDialogue.answers[i].makeDefault)
            {
                GameMaster.GM.DelayForAnswerSelect(answerButtons[i]); // leads to IEnumerator SelectButtonLater
                defaultSet = true;
            }// highlights button if Answer.makeDefault == true
        }// List all answer choices

        if (!defaultSet)
            GameMaster.GM.DelayForAnswerSelect(answerButtons[0]); // have to use GameMaster in order to use MonoBehavior

    }// Display Answers

    public IEnumerator SelectButtonLater(GameObject thisButton)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(thisButton);
    }

    public void SelectAnswer(int answerIndex)
    {
        // UI animation
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i != answerIndex)
                answerButtons[i].GetComponent<Animator>().SetTrigger("UnlistAnswer");
            else // i == answerIndex
            {
                answerButtons[i].GetComponent<Animator>().SetTrigger("SelectAnswer");
                answerButtons[i].GetComponent<Button>().interactable = false;
                // changed to interactable = true in button animation Out1, Out2, Out3, Out4, Out5
                // makes button unclickable but still visible with "disabled" color
                // Use Button.enabled = false to have same effect but not use "disabled" color of button
                buttonInSelectPosition = answerButtons[i]; // used to make sure next question does not get listed before this button goes back to its "Out" position
            }// move user's answer to "Selected" position in animator
        }// UI animation

        Answer selectedAnswer = currentDialogue.answers[answerIndex];
        currentSpeaker.AnswerSelected(selectedAnswer.indexDialogue); // restarts the cycle using the next dialogue from the speaker
    }

    public void ClearSelectedButton()
    {
        buttonInSelectPosition.GetComponent<Animator>().SetTrigger("LowerAnswer");
    }// removes answer button from selected position

    public void DisableAnswers()
    {
        answer1.SetActive(false);
        answer2.SetActive(false);
        answer3.SetActive(false);
        answer4.SetActive(false);
        answer5.SetActive(false);
    }

    public void SpeakerAction(string typedParameter)
    {
        // send to Speaker so method <typedParameter> can be executed
        currentSpeaker.ExecuteThisAction(typedParameter);
    }

    public void TurnOnContinueButton()
    {
        continueButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton); // ends up calling ContinueDialogue() after using GameMaster's monobehavior to call from OnClick
    }

    public void EndDialogue()
    {
        if (currentDialogue.nextDialogueIndex == -1) // directs dialogue to close
        {
            closeButton.SetActive(true);                        // Become visible
            closeButton.GetComponent<Button>().enabled = true;  // Become usable (necessary from FinishCoversation())
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(closeButton);
        }
        else // this dialouge directs to a new dialogue without asking a question
        {
            currentDialogue = currentSpeaker.allDialogues[currentDialogue.nextDialogueIndex];
            sentenceIndex = -1; // will be incremented before use
            TurnOnContinueButton();
            // Clicking continue button will call AdvanceSentence, which is just:
                    //sentenceIndex++;
                    //GameMaster.GM.DisplaySentence(currentDialogue.sentences[sentenceIndex]);
        }
    }

    public void FinishConversation()
    {
        currentSpeaker.ResumeLife();

        // disable buttons so they can't be affected in gameplay
        foreach (GameObject answerButton in answerButtons)
            answerButton.GetComponent<Button>().enabled = false;
        closeButton.GetComponent<Button>().enabled = false; // use enabled instead of SetActive(false) so that it stays visible during lowering animation

        HUDAnimator.SetBool("Raised", false);
        barsAnimator.SetBool("isOpen", false);
        nameAnimator.SetBool("isUp", false);
        if (buttonInSelectPosition != null)
            ClearSelectedButton();

        thePlayer.EnableControls();
        //thePlayer.controlsDisabled = false;
    }

}// Thanks, Brackeys!