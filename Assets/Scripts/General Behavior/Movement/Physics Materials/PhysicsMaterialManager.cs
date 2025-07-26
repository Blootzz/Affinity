using UnityEngine;

public class PhysicsMaterialManager : MonoBehaviour
{
    [SerializeField] PhysicsMaterial2D zeroFrictionBounce;
    [SerializeField] PhysicsMaterial2D playerDamaged;
    [SerializeField] PhysicsMaterial2D playerBlockSlide;

    public void SetRbZeroFrictionBounce()
    {
        GetComponent<Rigidbody2D>().sharedMaterial = zeroFrictionBounce;
    }

    public void SetRbPlayerDamaged()
    {
        GetComponent<Rigidbody2D>().sharedMaterial = playerDamaged;
    }

    public void SetRbPlayerBlockSlide()
    {
        GetComponent<Rigidbody2D>().sharedMaterial = playerBlockSlide;
    }
}
