using UnityEngine;
using System;

public class LedgeGrabChecker : MonoBehaviour
{
    public event Action LedgeGrabEvent;

    [Header("Reference Point Transforms")]
    [SerializeField] Transform InnerPoint;
    [SerializeField] Transform TopPoint;
    [SerializeField] Transform TargetPoint;

    [SerializeField] LayerMask layerMask;

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
        RaycastHit2D horizontalRay = Physics2D.Linecast(InnerPoint.position, TargetPoint.position, layerMask);
        RaycastHit2D verticalRay = Physics2D.Linecast(TopPoint.position, TargetPoint.position, layerMask);
        print("Horizontal distance: " + horizontalRay.distance);
        print("Vertical distance: " + verticalRay.distance);
        if (horizontalRay.distance > 0 && verticalRay.distance > 0)
        {
            print("Found a corner");
            return true;
        }
        else
        {
            print("No corner found");
            return false;
        }
    }

}//LedgeGrab
