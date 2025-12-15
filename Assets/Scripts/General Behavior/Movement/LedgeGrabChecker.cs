using UnityEngine;
using System;

public class LedgeGrabChecker : MonoBehaviour
{
    public event Action<Vector2> LedgeGrabEvent;

    [Header("Reference Point Transforms")]
    [SerializeField] Transform InnerPoint;
    [SerializeField] Transform TopPoint;
    [SerializeField] Transform TargetPoint;

    [SerializeField] LayerMask layerMask;

    // intended to run whenever this Gameobject is enabled
    private void FixedUpdate()
    {
        if (TryEvaluateLedgeGrab(out Vector2 cornerPosition))
        {
            LedgeGrabEvent?.Invoke(cornerPosition);
        }
    }

    /// <summary>
    /// Uses linecasts from upper and inner points to a outer-lower point to determine if there is a corner that can be grabbed onto
    /// </summary>
    /// <returns>true if cornerPos is nonzero</returns>
    bool TryEvaluateLedgeGrab(out Vector2 result)
    {
        // only return true if both RaycastHit2D results yield a distance.
        // If there is nothing, it measures 0. If it starts inside and object, it returns 0
     
        result = Vector2.zero; // default value that will never be used

        // Raycasts are separated to reduce Raycast calls. Check horizontal first then vertical
        RaycastHit2D horizontalRay = Physics2D.Linecast(InnerPoint.position, TargetPoint.position, layerMask);
        if (horizontalRay.distance > 0)
        {
            RaycastHit2D verticalRay = Physics2D.Linecast(TopPoint.position, TargetPoint.position, layerMask);
            if (verticalRay.distance > 0)
            {
                result = new Vector2(horizontalRay.point.x, verticalRay.point.y);
                return true;
            }
        }

        return false;
    }

}//LedgeGrab
