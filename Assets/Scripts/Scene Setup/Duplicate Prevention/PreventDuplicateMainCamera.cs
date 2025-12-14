using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateMainCamera : MonoBehaviour
{
    void Awake()
    {
        int numMainCameras = FindObjectsByType<PreventDuplicateMainCamera>(FindObjectsSortMode.None).Length;

        if (numMainCameras != 1)
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
