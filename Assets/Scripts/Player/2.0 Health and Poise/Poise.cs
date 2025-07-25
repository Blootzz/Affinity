using UnityEngine;
using System;

public class Poise : MonoBehaviour
{
    public float poise { get; private set; } = 100;
    public event Action PoiseDepletedEvent; // subscribed to by PlayerStateManager

    /// <summary>
    /// Deducts the positive damage number from health and runs death check
    /// </summary>
    public void DeductPoise(float poiseDamage)
    {
        poise -= poiseDamage;
        if (poise <= 0)
            PoiseDepleted();
    }

    void PoiseDepleted()
    {
        PoiseDepletedEvent();
    }
}
