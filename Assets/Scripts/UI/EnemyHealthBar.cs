using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform enemy;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] GameObject healthBar;

    Vector3 offset = new Vector3(0f, 1.33f, 0f);

    void Start()
    {
        transform.Rotate(0, 0, 0);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.text = health.ToString();
    }

    public void SetHealth(int health)
    {
        healthBar.SetActive(true);
        slider.value = health;
        healthText.text = health.ToString();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = enemy.position + offset;
        transform.position = targetPosition;
    }

    public void SetReloading(bool isReloading)
    {
        if (isReloading) ammoText.text = "Reloading...";
        else ammoText.text = "";
    }

    public void SetAmmo(int currentAmmo)
    {
        ammoText.text = "<sprite=0>" + currentAmmo;
    }
}
