using UnityEngine;
using System;

public class Health
{
    public float health { get; private set; } = 100;
    public event Action DeathEvent;

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
