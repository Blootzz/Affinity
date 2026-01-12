using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    [SerializeField] protected float damage;
    public float GetDamage() => damage;
    public float SetDamage(float newDamageValue) => damage = newDamageValue;
}
