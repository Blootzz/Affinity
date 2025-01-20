using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateShield : MonoBehaviour
{
    void Awake()
    {
        int numShields = FindObjectsOfType<PreventDuplicateShield>().Length;

        if (numShields != 1)
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
