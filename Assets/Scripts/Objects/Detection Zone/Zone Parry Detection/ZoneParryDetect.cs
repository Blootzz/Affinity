using UnityEngine;
using UnityEngine.Events;

public class ZoneParryDetect : MonoBehaviour
{
    public UnityEvent ParryCountedEvent;
    public UnityEvent ClearCountEvent;

    DetectZoneByTag detectZone;
    BlockParryManager foundBlockParryManager;

    private void Awake()
    {
        detectZone = GetComponent<DetectZoneByTag>();
    }

    private void OnEnable()
    {
        detectZone.TargetFoundEvent += DetectZone_TargetFoundEvent;
        detectZone.TargetExitedEvent += DetectZone_TargetExitedEvent;
    }

    private void OnDisable()
    {
        detectZone.TargetFoundEvent -= DetectZone_TargetFoundEvent;
        detectZone.TargetExitedEvent -= DetectZone_TargetExitedEvent;

        // extra unsubscribe from parry event in case this object is destroyed while still subscribed
        if (foundBlockParryManager != null)
        {
            foundBlockParryManager.SuccessfulParryEvent -= OnParry;
        }
    }

    private void DetectZone_TargetFoundEvent(GameObject obj)
    {
        foundBlockParryManager = detectZone.TrackedTargetPersistent.GetComponentInChildren<BlockParryManager>();

        // subscribe to blockParryManager's parry event
        foundBlockParryManager.SuccessfulParryEvent += OnParry;
    }

    private void DetectZone_TargetExitedEvent(GameObject obj)
    {
        // unsubscribe from parry event
        foundBlockParryManager.SuccessfulParryEvent -= OnParry;

        ClearCountEvent?.Invoke();
    }

    // must take faceRight to match event signature. Not used in most applications
    void OnParry(bool faceRight)
    {
        ParryCountedEvent?.Invoke();
    }
}
