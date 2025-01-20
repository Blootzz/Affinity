using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicatePlayer : MonoBehaviour
{
    void Awake()
    {
        int numPlayers = FindObjectsOfType<PreventDuplicatePlayer>().Length;

        if (numPlayers != 1)
        {
            // Destroy the extra instance
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
