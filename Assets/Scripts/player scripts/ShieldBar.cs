using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShieldBar : MonoBehaviour
{
    [SerializeField]  Slider slider;

    public void SetMaxValue(float value)
    {       

        slider.maxValue = value;
       
    }

    public void SetSlider(float health)
    {
        slider.value = health;
      
    }
}
