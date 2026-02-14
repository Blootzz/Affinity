using UnityEngine;

public class EnableOnDetectZone : MonoBehaviour
{
    [SerializeField] GameObject objectToEnable;
    DetectZoneByTag detectZone;

    [SerializeField] bool disableOnExit = false;

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
        objectToEnable.SetActive(true);
    }

    private void DetectZone_TargetExitedEvent(GameObject obj)
    {
        if (disableOnExit)
            objectToEnable.SetActive(false);
    }
}
