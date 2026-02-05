using UnityEngine;

public class BaseProjectileBehaviour : MonoBehaviour
{

    protected Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void Start()
    {
        Launch();
    }

    private void FixedUpdate()
    {
        InFixedUpdate();
    }

    public virtual void Launch()
    {
        print("default projectile launch behaviour");
    }

    public virtual void InFixedUpdate() { }

    public virtual void Reflect() { }
}
