using UnityEngine;
using System;

public class DetectZoneByTag : MonoBehaviour
{
    public string searchForThisTag;
    public event Action<GameObject> TargetFoundEvent;
    public event Action<GameObject> TargetExitedEvent;
    [SerializeField] Vector2 startingWorldPos;

     bool targetInZone = false;
    public bool TargetInZone => targetInZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            //print("Player entered attack zone");
            TargetFoundEvent?.Invoke(collision.gameObject);
            targetInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            TargetExitedEvent?.Invoke(collision.gameObject);
            targetInZone = false;
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
