using UnityEngine;
using UnityEngine.UIElements;

public class LaunchDynamic : BaseProjectileBehaviour
{
    public override void Launch()
    {
        Vector3 launchAngle = new Vector3((projectile.GetAttackFaceRight() ? 1 : -1) * (projectile.GetIsParried() ? -1 : 1) * projectile.GetAngle().x, projectile.GetAngle().y, 0);
        print("Adding force at angle: " + launchAngle);
        GetComponent<Rigidbody2D>().AddForce(projectile.GetSpeed() * launchAngle);
    }

    public override void Reflect()
    {
        Launch();
    }
}
