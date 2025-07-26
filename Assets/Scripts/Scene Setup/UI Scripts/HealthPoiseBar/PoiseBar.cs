using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoiseBar : MonoBehaviour
{
    Slider slider;
    [SerializeField] Poise trackedPoise; // assign this in inspector

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        trackedPoise.PoiseChangedEvent += OnPoiseChanged;
    }
    private void OnDisable()
    {
        trackedPoise.PoiseChangedEvent -= OnPoiseChanged;
    }

    void OnPoiseChanged(float updatedPoise)
    {
        slider.value = updatedPoise;
    }
}
