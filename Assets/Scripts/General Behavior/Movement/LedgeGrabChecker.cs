using UnityEngine;
using System;

public class LedgeGrabChecker : MonoBehaviour
{
    public event Action LedgeGrabEvent;

    [Header("Reference Point Transforms")]
    Transform InnerPoint;
    Transform TopPoint;
    Transform TargetPoint;

    private void OnEnable()
    {
        
    }

    private void FixedUpdate()
    {
        if (EvaluateLedgeGrab())
        {
            print("Ledge found");
        }
    }

    bool EvaluateLedgeGrab()
    {
        print("Evaluate ledge grab here");
        return true;
    }

}//LedgeGrab
