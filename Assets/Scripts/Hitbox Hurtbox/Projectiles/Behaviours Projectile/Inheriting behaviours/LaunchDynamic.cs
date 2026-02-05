using UnityEngine;
using UnityEngine.UIElements;

public class LaunchDynamic : BaseProjectileBehaviour
{
    public override void Launch()
    {
        print("Adding force at angle: " + projectile.GetAngle());
        GetComponent<Rigidbody2D>().AddForce(projectile.GetSpeed() * (projectile.GetAttackFaceRight() ? 1 : -1) * projectile.GetAngle());
    }

    public override void Reflect()
    {
        projectile.SetAngle(new Vector2(projectile.GetAngle().x * -1, projectile.GetAngle().y));
        Launch();
    }
}
