using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
   Slider slider;

    public void SetMaxValue(int value)
    {
        slider = GetComponent<Slider>();

        slider.maxValue = value;
        slider.value = value;
    }

    public void SetSlider(int health)
    {
        slider.value = health;
    }
    
}
