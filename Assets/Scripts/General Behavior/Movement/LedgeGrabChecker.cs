using UnityEngine;
using System;

public class LedgeGrabChecker : MonoBehaviour
{
    public event Action LedgeGrabEvent;

    BoxCollider2D detectZone;

    private void Awake()
    {
        detectZone = GetComponent<BoxCollider2D>();
    }



}//LedgeGrab
