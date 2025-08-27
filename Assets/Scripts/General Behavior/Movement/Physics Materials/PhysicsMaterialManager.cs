using UnityEngine;

public class PhysicsMaterialManager : MonoBehaviour
{
    [SerializeField] PhysicsMaterial2D zeroFrictionBounce;
    [SerializeField] PhysicsMaterial2D playerDamaged;
    [SerializeField] PhysicsMaterial2D playerBlockSlide;
    [SerializeField] PhysicsMaterial2D highFriction;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetRbZeroFrictionBounce()
    {
        rb.sharedMaterial = zeroFrictionBounce;
    }

    public void SetRbPlayerDamaged()
    {
        rb.sharedMaterial = playerDamaged;
    }

    public void SetRbPlayerBlockSlide()
    {
        rb.sharedMaterial = playerBlockSlide;
    }

    public void SetRbHighFriction()
    {
        rb.sharedMaterial = highFriction;
    }

}
