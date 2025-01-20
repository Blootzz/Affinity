using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{

    public static bool IsPointWithinCollider(Collider collider, Vector3 point)
    {
        return (collider.ClosestPoint(point) - point).sqrMagnitude < Mathf.Epsilon * Mathf.Epsilon;
    }

}// Utility class
