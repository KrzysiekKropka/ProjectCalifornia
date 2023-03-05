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
    public TMP_Text moneyText;
    public TMP_Text killsText;
    public TMP_Text weaponText;
    public TMP_Text ammoText;

    public void ActiveMode(bool active)
    {
        gameObject.SetActive(active);
    }

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

    //KK: te funkcje sa uzywane w Player.cs

    public void SetKills (int kills)
    {
        killsText.text = "Kills: " + kills;
    }

    public void SetExperiencePoints(int experiencePoints)
    {
        experiencePointsText.text = "XP: " + experiencePoints;
    }

    public void SetMoney(int money)
    {
        moneyText.text = "Money: " + money + "$";
    }

    public void SetWeaponName(string weaponName)
    {
        weaponText.text = "Equipped " + weaponName + "!";
    }

    public void SetAmmo(int currentAmmo, int reserveAmmo)
    {
        ammoText.text = currentAmmo + "/" + reserveAmmo;
    }
}
