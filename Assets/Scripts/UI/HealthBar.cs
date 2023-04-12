using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Image weaponIcon;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text experiencePointsText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text killsText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text reloadingText;
    [SerializeField] Sprite Pistol;
    [SerializeField] Sprite Deagle;
    [SerializeField] Sprite MP5;
    [SerializeField] Sprite Shotgun;
    [SerializeField] Sprite AK47;
    [SerializeField] RectTransform HealthBarOnly;


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

    public void SetMaxStamina(float stamina)
    {
        staminaSlider.gameObject.SetActive(true);
        HealthBarOnly.anchoredPosition = new Vector2(0, 35);
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    }

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }

    public void HideStamina()
    {
        staminaSlider.gameObject.SetActive(false);
        HealthBarOnly.anchoredPosition = new Vector2(0, 5);
    }

    //KK: te funkcje sa uzywane w Player.cs

    public void SetKills (int kills)
    {
        killsText.text = "Kills: " + kills;
    }

    public void SetExperiencePoints(int experiencePoints)
    {
        experiencePointsText.text = experiencePoints + "<sprite=0>";
    }

    public void SetMoney(int money)
    {
        moneyText.text = "<sprite=0>" + money + "$";
    }

    public void SetAmmo(int currentAmmo, int reserveAmmo)
    {
        ammoText.text = currentAmmo + "/" + reserveAmmo;
    }

    public void SetWeaponIcon(int equippedWeaponID)
    {
        switch (equippedWeaponID) 
        { 
            case 0:
                weaponIcon.sprite = Pistol;
                break;
            case 1:
                weaponIcon.sprite = Deagle;
                break;
            case 2:
                weaponIcon.sprite = MP5;
                break;
            case 3:
                weaponIcon.sprite = Shotgun;
                break;
            case 4:
                weaponIcon.sprite = AK47;
                break;
            default:
                break;
        }
    }

    public void SetReloading(bool isReloading)
    {
        if (isReloading) reloadingText.text = "Reloading...";
        else reloadingText.text = "";
    }
}
