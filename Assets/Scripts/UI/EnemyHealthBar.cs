using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Transform enemy;
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] GameObject healthBar;

    private IEnumerator dialogueCoroutine;

    Vector3 offset = new Vector3(0f, 1.33f, 0f);

    void Start()
    {
        dialogueText.gameObject.SetActive(false);
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
        if (isReloading) ammoText.text = "Reloading";
        else ammoText.text = "";
    }

    public void SetAmmo(int currentAmmo)
    {
        ammoText.text = currentAmmo + "<sprite=0>";
    }

    public void Dialogue(string text)
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = text;
        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = DialogueInterval();
        StartCoroutine(dialogueCoroutine);
    }

    private IEnumerator DialogueInterval()
    {
        yield return new WaitForSeconds(2.5f);
        dialogueText.gameObject.SetActive(false);
    }
}
