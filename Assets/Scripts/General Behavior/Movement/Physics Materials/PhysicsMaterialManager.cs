using UnityEngine;

public class PhysicsMaterialManager : MonoBehaviour
{
    [SerializeField] PhysicsMaterial2D zeroFrictionBounce;
    [SerializeField] PhysicsMaterial2D damaged;
    [SerializeField] PhysicsMaterial2D blockSlide;
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

    public void SetRbDamaged()
    {
        rb.sharedMaterial = damaged;
    }

    public void SetRbBlockSlide()
    {
        rb.sharedMaterial = blockSlide;
    }

    public void SetRbHighFriction()
    {
        rb.sharedMaterial = highFriction;
    }

}
