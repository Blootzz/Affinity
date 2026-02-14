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

    /// <summary>
    /// reference of object found by detectZone. Does not go null after object leaves zone
    /// </summary>
    [HideInInspector] public GameObject TrackedTargetPersistent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            //print("Player entered attack zone");
            targetInZone = true;
            TrackedTargetPersistent = collision.gameObject;
            TargetFoundEvent?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(searchForThisTag))
        {
            targetInZone = false;
            TargetExitedEvent?.Invoke(collision.gameObject);
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
