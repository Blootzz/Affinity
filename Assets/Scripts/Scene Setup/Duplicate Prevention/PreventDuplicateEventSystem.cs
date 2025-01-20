using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateEventSystem : MonoBehaviour
{
    void Awake()
    {
        int numEventSystems = FindObjectsOfType<PreventDuplicateEventSystem>().Length;

        if (numEventSystems != 1)
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
