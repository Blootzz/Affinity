using UnityEngine;

public class LaunchDynamic : MonoBehaviour
{
    Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(projectile.GetSpeed() * (projectile.GetAttackFaceRight() ? 1 : -1) * projectile.GetAngle());
    }
}
