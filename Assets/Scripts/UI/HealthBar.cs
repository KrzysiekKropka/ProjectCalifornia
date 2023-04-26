using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] AudioClip messageBoxClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image weaponIcon;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text experiencePointsText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text killsText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text reloadingText;
    [SerializeField] TMP_Text messageBox;
    [SerializeField] Sprite Pistol;
    [SerializeField] Sprite Deagle;
    [SerializeField] Sprite MP5;
    [SerializeField] Sprite Shotgun;
    [SerializeField] Sprite AK47;
    [SerializeField] RectTransform HealthBarOnly;
    [SerializeField] Gradient gradient;
    [SerializeField] Gradient vignetteGradient;
    [SerializeField] Image fill;
    [SerializeField] Volume volume;
    private Vignette vignette;

    private IEnumerator messageBoxCoroutine;

    void OnEnable()
    {
        volume.profile.TryGet(out vignette);

        vignette.color.value = vignetteGradient.Evaluate(1f);
        if (messageBoxCoroutine != null) StopCoroutine(messageBoxCoroutine);
        messageBoxCoroutine = MessageBoxInterval();
        StartCoroutine(messageBoxCoroutine);
    }

    public void ActiveMode(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;

        //fill.color = gradient.Evaluate(1f);
        vignette.color.value = vignetteGradient.Evaluate(1f);
        //vignette.intensity.value = Mathf.InverseLerp(2.5f, -2f, slider.normalizedValue);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.text = health.ToString();
        //print(slider.normalizedValue);

        //fill.color = gradient.Evaluate(slider.normalizedValue);
        vignette.color.value = vignetteGradient.Evaluate(slider.normalizedValue);
        //vignette.intensity.value = Mathf.InverseLerp(2.5f, -2f, slider.normalizedValue);
        //print(Mathf.InverseLerp(2.5f, -2f, slider.normalizedValue));
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

    public void ArenaMode()
    {
        killsText.gameObject.SetActive(true);
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

    public void MessageBox(string message)
    {
        messageBox.text += message + "\r\n";
        messageBox.gameObject.SetActive(true);
        audioSource.PlayOneShot(messageBoxClip);
        if (messageBox.textInfo.lineCount >= 5)
        {
            string oldText = messageBox.text;
            int index = oldText.IndexOf(System.Environment.NewLine);
            string newText = oldText.Substring(index + System.Environment.NewLine.Length);
            messageBox.text = newText;
        }
        if (messageBoxCoroutine != null) StopCoroutine(messageBoxCoroutine);
        messageBoxCoroutine = MessageBoxInterval();
        StartCoroutine(messageBoxCoroutine);
    }

    public IEnumerator MessageBoxInterval()
    {
        yield return new WaitForSecondsRealtime(8f);
        while(messageBox.text != "")
        {
            yield return new WaitForSecondsRealtime(1f);
            string oldText = messageBox.text;
            int index = oldText.IndexOf(System.Environment.NewLine);
            string newText = oldText.Substring(index + System.Environment.NewLine.Length);
            messageBox.text = newText;
        }
        messageBox.gameObject.SetActive(false);
    }
}
