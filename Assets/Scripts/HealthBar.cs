using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerEntity player;

    public void SetMaxHealt(float haelth)
    {
        slider.maxValue = haelth;
        slider.value = haelth;
    }

    public void SetHealth(float haelth)
    {
        slider.value = haelth;
    }
}
