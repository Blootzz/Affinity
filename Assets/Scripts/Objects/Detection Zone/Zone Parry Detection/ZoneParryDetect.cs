using UnityEngine;

public class ZoneParryDetect : MonoBehaviour
{
    DetectZoneByTag detectZone;

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
    }

    private void DetectZone_TargetFoundEvent(GameObject obj)
    {

    }

    private void DetectZone_TargetExitedEvent(GameObject obj)
    {

    }
}
