using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text healthText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health.ToString();
    }

    public void SetHealth(int health)
    {
        if (health < 100) gameObject.SetActive(true);
        slider.value = health;
        healthText.text = health.ToString();
    }
}
