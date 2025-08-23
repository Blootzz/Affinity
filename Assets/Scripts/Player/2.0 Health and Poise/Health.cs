using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] public float health { get; private set; } = 100;
    public event Action DeathEvent; // subscribed to by PlayerStateManager
    public event Action<float> HealthChangedEvent; // subscribed to by HealthBar

    /// <summary>
    /// Deducts the positive damage number from health and runs death check
    /// </summary>
    public void DeductHealth(float damage)
    {
        health -= damage;
        HealthChangedEvent(health);

        if (health <= 0)
            Die();
    }

    void Die()
    {
        DeathEvent?.Invoke();
    }
}
