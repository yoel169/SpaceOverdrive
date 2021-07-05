using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField]  Slider slider;

    public void SetMaxValue(int value)
    {

        slider.maxValue = value;
        
    }

    public void SetSlider(int health)
    {
        slider.value = health;
    }
    
}
