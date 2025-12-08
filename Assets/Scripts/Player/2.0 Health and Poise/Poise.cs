using UnityEngine;
using System;

public class Poise : MonoBehaviour
{
    [SerializeField] float maxPoise = 100;
    [SerializeField] float missedParryPoisePenalty = 20;

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

    /// <summary>
    /// Used by player when player attempts to parry but doesn't parry anything
    /// </summary>
    public void PlayerDeductMissedParryPenalty()
    {
        DeductPoise(missedParryPoisePenalty);
    }

    /// <summary>
    /// Used by enemy when one of its attacks is parried
    /// </summary>
    public void EnemyDeductPoiseFromPlayerParry(float parryPoiseDamage)
    {
        DeductPoise(parryPoiseDamage);
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
        PoiseDepletedEvent?.Invoke();
    }
}
