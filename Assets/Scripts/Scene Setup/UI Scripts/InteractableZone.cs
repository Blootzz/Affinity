using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableZone : MonoBehaviour
{
    bool isPlayerHere = false;

    private void Update()
    {
        if (isPlayerHere && CheckAvailabilityOfTarget() && ReferenceEquals(GameMaster.GM.interactableTargets[0], gameObject)) // if player is within zone and target is ready and 
        {
            transform.GetChild(0).gameObject.SetActive(true); // Activates bouncing arrow above Speaker

            if (Input.GetKeyDown(GameMaster.GM.controlManager.interactKey) && !GameMaster.GM.thePlayer.controlsDisabled)
            {
                TriggerInteraction();
                transform.GetChild(0).gameObject.SetActive(false);
            }// when Interact key is pressed and controls are not disabled
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }// conditions are not met to have object highlighted by bouncing arrow
    }

    bool CheckAvailabilityOfTarget()
    {
        if (transform.parent.GetComponent<Speaker>() != null)
        {
            if (transform.parent.GetComponent<Speaker>().isAvailableToTalk)
                return true;
            else
                return false;
        }// if parent is a speaker
        if (GetComponentInParent<Interactable>() != null)
        {
            if (GetComponentInParent<Interactable>().isAvailableToInteract)
                return true;
            else
                return false;
        }// if parent is an Interactable

        return true; // by default
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameMaster.GM.interactableTargets.Add(gameObject); // adds this game object to the list of currently available targets
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameMaster.GM.interactableTargets.Remove(gameObject); // removes this game object from the list of currently available targets
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerHere = false;
        }
    }

    void TriggerInteraction()
    {
        if (GetComponentInParent<Speaker>() != null)
        {
            GetComponentInParent<Speaker>().TriggerDialogueManually();
        }// if owner of InteractableZone is a speaker
        else
        {
            if (GetComponentInParent<Interactable>() != null)
                GetComponentInParent<Interactable>().Execute();
        }// if owner of InteractableZone is an Interactable
    }
}
