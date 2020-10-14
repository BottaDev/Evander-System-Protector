using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerEntity player;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealt(float haelth)
    {
        slider.maxValue = haelth;
        slider.value = haelth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float haelth)
    {
        slider.value = haelth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
