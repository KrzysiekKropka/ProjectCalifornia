using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AIBrain : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] GameObject bloodPoolEffect;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] Rigidbody2D rb;
    private GameObject player;
    private GameObject damagePopup;

    bool readyToShoot = false;
    int maxHealth = 100;
    int currentHealth;
    public int dropXP;
    public int dropMoney;
    int summedDamage;
    float currentTime, currentTimeBloodPool;
    float aimAngle;

    Vector3 aimDirection;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        aimDirection = player.transform.position - transform.position;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void FixedUpdate()
    {
        rb.rotation = aimAngle;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //KK:Blagam nie dotykaj tego kodu ponizej, jest on tak kurwa niestabilny ze lekka zmiana kompletnie rozpierdoli dzialanie licznika. To jest niesamowite ze to w ogole dziala.
        if (Time.time - currentTime > 0.5f)
        {
            summedDamage = 0;
        }

        summedDamage += damage;

        if (Time.time - currentTime > 0.1f)
        {
            damagePopup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            damagePopup.GetComponent<DamagePopup>().SetDamageText(summedDamage);
        }
        else
        {
            damagePopup.GetComponent<DamagePopup>().SetDamageText(summedDamage);
        }
        if (currentHealth <= 0)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player)
            {
                player.SetXP(dropXP);
                player.AddKill();
            }
            Destroy(transform.parent.gameObject);
            if (Time.time - currentTimeBloodPool > .01f)
            {
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
                currentTimeBloodPool = Time.time;
            }
        }
        healthBar.SetHealth(currentHealth);
        currentTime = Time.time;
    }
}
