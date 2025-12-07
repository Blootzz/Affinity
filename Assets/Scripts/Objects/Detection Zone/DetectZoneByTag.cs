using UnityEngine;
using System;

public class DetectZoneByTag : MonoBehaviour
{
    public string searchForThisTag;
    public event Action<GameObject> TargetFoundEvent;
    [SerializeField] Vector2 startingWorldPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            TargetFoundEvent?.Invoke(collision.gameObject);
        }
    }

    // don't move even if parent is moving
    private void Start()
    {
        startingWorldPos = transform.position;
    }
    private void Update()
    {
        transform.position = startingWorldPos;
    }
}
