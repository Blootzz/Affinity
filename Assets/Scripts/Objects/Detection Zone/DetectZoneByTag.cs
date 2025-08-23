using UnityEngine;
using System;

public class DetectZoneByTag : MonoBehaviour
{
    public string searchForThisTag;
    public event Action<GameObject> TargetFoundEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            TargetFoundEvent?.Invoke(collision.gameObject);
        }
    }
}
