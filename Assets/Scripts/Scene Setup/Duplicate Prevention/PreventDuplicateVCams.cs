using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateVCams : MonoBehaviour
{
    void Awake()
    {
        int numVCams = FindObjectsOfType<PreventDuplicateVCams>().Length;

        if (numVCams != 1)
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
