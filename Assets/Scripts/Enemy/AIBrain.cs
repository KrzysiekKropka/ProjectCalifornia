using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class AIBrain : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject bloodPoolEffect;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] GameObject moneyDropPrefab;
    [SerializeField] GameObject ammoDropPrefab;
    [SerializeField] Rigidbody2D rb;

    private GameObject player;
    private GameObject damagePopup;
    private AIPath ai;

    [SerializeField] float speed = 4f; 
    [SerializeField] int maxHealth = 100;
    bool readyToShoot = false;
    bool seenPlayer = true;
    int currentHealth;
    public int dropXP;
    public int dropMoney;
    public int dropHP;
    int summedDamage;
    float currentTime, currentTimeBloodPool;
    float aimAngle;

    Vector3 aimDirection;

    void Start()
    {
        deadBody.SetActive(false);
        ai = GetComponent<AIPath>();
        currentHealth = maxHealth;
        healthBar.GetComponent<EnemyHealthBar>().SetMaxHealth(maxHealth);
        player = GameObject.FindGameObjectWithTag("Player");
        ai.maxSpeed = speed;
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

    void LateUpdate()
    {
        if (seenPlayer) ai.destination = player.transform.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //KK:Blagam nie dotykaj tego kodu ponizej, jest on tak kurwa niestabilny ze lekka zmiana kompletnie rozpierdoli dzialanie licznika. To jest niesamowite ze to w ogole dziala.
        if (Time.time - currentTime > 0.75f)
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
            deadBody.SetActive(true);
            Destroy(gameObject);
            Destroy(healthBar);
            if (Time.time - currentTimeBloodPool > .01f)
            {
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
                GameObject droppedMoney = Instantiate(moneyDropPrefab, transform.position + new Vector3(-0.75f, 0f, 0f), Quaternion.identity);
                droppedMoney.GetComponent<MoneyDrop>().money = dropMoney;
                GameObject droppedAmmo = Instantiate(ammoDropPrefab, transform.position + new Vector3(0.75f, 0f, 0f), Quaternion.identity);
                droppedAmmo.GetComponent<AmmoDrop>().hp = dropHP;
                currentTimeBloodPool = Time.time;
            }
        }
        healthBar.GetComponent<EnemyHealthBar>().SetHealth(currentHealth);
        currentTime = Time.time;
    }
}
