using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    private void Awake() // Needs to set reference of slider before Anslem.Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void SetHealthSlider(float health)
    {
        slider.value = health;
    }    
}
