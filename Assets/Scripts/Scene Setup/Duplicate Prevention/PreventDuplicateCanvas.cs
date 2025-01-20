using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateCanvas : MonoBehaviour
{
    void Awake()
    {
        int numCanvases = FindObjectsOfType<PreventDuplicateCanvas>().Length;

        if (numCanvases != 1)
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
