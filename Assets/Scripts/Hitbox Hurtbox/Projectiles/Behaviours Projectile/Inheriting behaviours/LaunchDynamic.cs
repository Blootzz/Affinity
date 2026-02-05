using UnityEngine;

public class LaunchDynamic : BaseProjectileBehaviour
{
    public override void Launch()
    {
        GetComponent<Rigidbody2D>().AddForce(projectile.GetSpeed() * (projectile.GetAttackFaceRight() ? 1 : -1) * projectile.GetAngle());
    }
}
