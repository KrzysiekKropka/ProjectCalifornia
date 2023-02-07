using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TMP_Text healthText;
    public TMP_Text experiencePointsText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health.ToString();
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.text = health.ToString();
    }

    public void SetExperiencePoints(int experiencePoints)
    {
        experiencePointsText.text = "XP: " + experiencePoints;
    }
}
