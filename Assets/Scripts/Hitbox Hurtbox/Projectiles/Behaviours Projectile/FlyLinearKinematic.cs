using Codice.CM.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyLinearKinematic : MonoBehaviour
{
    Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void Start()
    {
        projectile.Rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public virtual void FixedUpdate()
    {
        transform.position += Time.fixedDeltaTime * 60 * projectile.Speed * (projectile.AttackFaceRight ? 1 : -1) * projectile.GetAngle;
    }// basic linear movement
}
