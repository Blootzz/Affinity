using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldLedge : MonoBehaviour
{
    Player thePlayer;
    Shield theShield;

    private void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        theShield = gameObject.transform.parent.gameObject.transform.parent.GetComponent<Shield>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("LedgeGrabCheck"))
            theShield.isPlayerHangingOnPlatform = true;
    }

    private void OnDisable()
    {
        if (theShield.isPlayerHangingOnPlatform) // if player is hanging on platform
        {
            thePlayer.onLedge = false;
            theShield.isPlayerHangingOnPlatform = false;
            thePlayer.GetComponent<Rigidbody2D>().isKinematic = false;
            thePlayer.GetComponent<Animator>().Play(thePlayer.AorUFalling);
        }
    }
}
