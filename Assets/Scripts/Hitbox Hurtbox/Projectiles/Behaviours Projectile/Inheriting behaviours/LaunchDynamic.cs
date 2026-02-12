using UnityEngine;
using UnityEngine.UIElements;

public class LaunchDynamic : BaseProjectileBehaviour
{
    public override void Launch()
    {
        Vector3 launchAngle = new Vector3((projectile.GetAttackFaceRight() ? 1 : -1) * (projectile.GetIsParried() ? -1 : 1) * projectile.GetAngle().x, projectile.GetAngle().y, 0);
        print("Adding force at angle: " + (projectile.GetSpeed() * launchAngle));
        GetComponent<Rigidbody2D>().AddForce(projectile.GetSpeed() * launchAngle);
    }

    // this gets called before the physics system processes the collision, so the physics system overrides anything here after processing collision
    public override void Reflect()
    {
        Launch();
        gameObject.layer = LayerMask.NameToLayer("PlayerHitbox");
    }

}
