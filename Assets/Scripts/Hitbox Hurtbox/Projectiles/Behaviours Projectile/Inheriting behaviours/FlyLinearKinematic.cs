using Codice.CM.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyLinearKinematic : BaseProjectileBehaviour
{
    public override void Launch()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public override void InFixedUpdate()
    {
        transform.position += Time.fixedDeltaTime * 60 * projectile.GetSpeed() * (projectile.GetAttackFaceRight() ? 1 : -1) * projectile.GetAngle();
    }// basic linear movement
}
