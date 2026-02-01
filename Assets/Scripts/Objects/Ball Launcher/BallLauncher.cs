using UnityEngine;
using System;
using System.Collections;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] DetectZoneByTag detectZone;
    [SerializeField] float secondsBetweenShots;

    Transform playerTransform;

    private void OnEnable()
    {
        detectZone.TargetFoundEvent += DetectZone_TargetFoundEvent;
        detectZone.TargetExitedEvent += DetectZone_TargetExitedEvent;
    }
    
    private void DetectZone_TargetFoundEvent(GameObject obj)
    {
        playerTransform = obj.transform;
        StartCoroutine(nameof(PauseAndShoot));
    }

    private void DetectZone_TargetExitedEvent(GameObject obj)
    {
        StopAllCoroutines();
    }

    IEnumerator PauseAndShoot()
    {
        while (detectZone.TargetInZone)
        {
            yield return new WaitForSeconds(secondsBetweenShots);

            /*Projectile firedProjectile = */
            Instantiate(projectile, this.transform.position, Quaternion.identity);
        }
    }
}
