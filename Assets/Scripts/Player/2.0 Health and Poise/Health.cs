using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] public float health { get; private set; } = 100;
    public event Action DeathEvent; // subscribed to by PlayerStateManager

    /// <summary>
    /// Deducts the positive damage number from health and runs death check
    /// </summary>
    public void DeductHealth(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        DeathEvent();
    }
}
