using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    float waterDamage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>()) // returns true if has component Player
        {
            GameMaster.GM.respawnManager.ExecuteVoidOut();
            GameMaster.GM.thePlayer.TakeHit(waterDamage, new Vector2(1, 0), true);
        }
    }
}
