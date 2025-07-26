using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    [SerializeField] Health trackedHealth; // set this in inspector

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    private void OnEnable()
    {
        trackedHealth.HealthChangedEvent += OnHealthChanged;
    }
    private void OnDisable()
    {
        trackedHealth.HealthChangedEvent -= OnHealthChanged;
    }

    void OnHealthChanged(float updatedHealth)
    {
        slider.value = updatedHealth;
    }

}
