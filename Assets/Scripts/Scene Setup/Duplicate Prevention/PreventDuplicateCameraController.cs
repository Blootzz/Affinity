using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDuplicateCameraController : MonoBehaviour
{
    void Awake()
    {
        int numCameraControllers = FindObjectsOfType<PreventDuplicateCameraController>().Length;

        if (numCameraControllers != 1)
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
