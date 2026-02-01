using UnityEngine;
using System;
using System.Collections;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] DetectZoneByTag detectZone;
    [SerializeField] float secondsBetweenShots;

    [Header("Random Speed")]
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    [Header("Random Angle")]
    [SerializeField] float minAngleDeg = 0;
    [SerializeField] float maxAngleDeg = 0;

    Transform playerTransform;

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

            Projectile firedProjectile = Instantiate(projectile, this.transform.position, Quaternion.identity).GetComponent<Projectile>();
            // spawning projectile. Can I modify it before it calls start? --> Yes

            bool faceRight;
            if (playerTransform.position.x > this.transform.position.x)
                faceRight = true;
            else
                faceRight = false;
            firedProjectile.SetAttackFaceRight(faceRight);

            firedProjectile.SetSpeed(GenerateRandomLaunchSpeed());

            firedProjectile.SetAngle(GenerateRandomLaunchAngle());
        }
    }

    float GenerateRandomLaunchSpeed()
    {
        return UnityEngine.Random.Range(minSpeed, maxSpeed);
    }

    Vector2 GenerateRandomLaunchAngle()
    {
        float randomDegrees = UnityEngine.Random.Range(minAngleDeg, maxAngleDeg);
        float rad = randomDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}
