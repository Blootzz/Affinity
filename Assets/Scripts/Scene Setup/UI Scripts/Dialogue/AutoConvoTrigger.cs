using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoConvoTrigger : MonoBehaviour
{
    bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenTriggered)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                hasBeenTriggered = true;
                GetComponentInParent<Speaker>().PlayerNearby();
            }// player enters
        }// has not been triggered yet
    }// OnTriggerEnter2D

}
