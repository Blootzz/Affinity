using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void SetStaminaSlider(float stamina)
    {
        slider.value = stamina;
    }
}
