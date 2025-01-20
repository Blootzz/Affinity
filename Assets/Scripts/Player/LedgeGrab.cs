using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    Player thePlayer;
    Vector2 ledgePos;
    bool regrabPrevention = false;

    private void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }

    // Couldn't get gameObject.GetComponent<CircleCollider2D>().enabled = false to work so I made a new variable Player.ledgeGrabActive
    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.CompareTag("Ledge") && thePlayer.ledgeGrabActive)
    //    {
    //        //print("FOUND. ledgeGrabActive = " + thePlayer.ledgeGrabActive);
    //        thePlayer.ledgeGrabActive = false;
    //        pos = collider.gameObject.transform.position;
    //        thePlayer.GrabLedge(pos);
    //    }
        
    //}// OnTriggerEnter2D

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Logic handling rope is done in Rope.cs

        if (thePlayer.ledgeGrabActive && collider.gameObject.CompareTag("Ledge") && !regrabPrevention)
        {
            regrabPrevention = true;
            thePlayer.ledgeGrabActive = false;
            ledgePos = collider.gameObject.transform.position;
            thePlayer.GrabLedge(ledgePos);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ledge"))
        {
            regrabPrevention = false;
        }
    }

}// LedgeGrab
