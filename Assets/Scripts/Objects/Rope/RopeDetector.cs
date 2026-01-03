using UnityEngine;
using System;

public class RopeDetector : MonoBehaviour
{
    public event Action<Rope> RopeFoundEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Rope>(out Rope ropeController))
            RopeFoundEvent?.Invoke(ropeController);
    }
}
