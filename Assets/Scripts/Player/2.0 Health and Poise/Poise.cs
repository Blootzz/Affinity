using UnityEngine;
using System;

public class Poise : MonoBehaviour
{
    [SerializeField] float maxPoise = 100;
    public float poise { get; private set; }
    public event Action PoiseDepletedEvent; // subscribed to by PlayerStateManager
    public event Action<float> PoiseChangedEvent; // subscribed to by PoiseBar

    private void Awake()
    {
        poise = maxPoise;
    }

    /// <summary>
    /// Deducts the positive damage number from health and runs death check
    /// </summary>
    public void DeductPoise(float poiseDamage)
    {
        poise -= poiseDamage;
        PoiseChangedEvent(poise);

        if (poise <= 0)
            PoiseDepleted();
    }

    public void AddPoise(float poiseAdded)
    {
        poise += poiseAdded;
        if (poise > maxPoise)
            poise = maxPoise;

        PoiseChangedEvent(poise);
    }

    void PoiseDepleted()
    {
        PoiseDepletedEvent();
    }
}
