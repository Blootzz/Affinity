using System;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroyEvent : MonoBehaviour
{
    public UnityEvent DestructionEvent;

    private void OnDestroy()
    {
        DestructionEvent?.Invoke();
    }
}
