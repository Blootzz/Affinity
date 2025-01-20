using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickRespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            GameMaster.GM.respawnManager.UpdateRespawnPoint(transform.position);
    }
}
