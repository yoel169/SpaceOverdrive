using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShieldBar : MonoBehaviour
{
    Slider slider;
    public void SetMaxValue(float value)
    {
        slider = GetComponent<Slider>();

        slider.maxValue = value;
        slider.value = value;
    }

    public void SetSlider(float health)
    {
        slider.value = health;
    }
}
