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
    [SerializeField] GameObject medkitPrefab;
    [SerializeField] Rigidbody2D rb;

    private GameObject player;
    private GameObject damagePopup;
    private AIPath ai;

    [SerializeField] float speed = 4f; 
    public int maxHealth = 100;

    bool playerDetected = true;
    bool shotBefore = false;

    int currentHealth;
    public int dropXP;
    public int dropMoney;
    public int dropHP;
    int summedDamage;
    float currentTime, currentTimeBloodPool;
    float aimAngle;

    RaycastHit hit;

    Vector3 aimDirection;

    void Start()
    {
        deadBody.SetActive(false);
        ai = GetComponent<AIPath>();
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
        if (playerDetected) ai.destination = player.transform.position;
    }

    public void TakeDamage(int damage)
    {
        if(!shotBefore)
        {
            currentHealth = maxHealth;
            healthBar.GetComponent<EnemyHealthBar>().SetMaxHealth(maxHealth);
            shotBefore = true;
        }
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
            deadBody.SetActive(true);
            Destroy(gameObject);
            Destroy(healthBar);
            if (Time.time - currentTimeBloodPool > .01f)
            {
                if (player)
                {
                    player.SetXP(dropXP);
                    player.AddKill();
                }
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
                GameObject droppedMoney = Instantiate(moneyDropPrefab, transform.position + new Vector3(-0.75f, -0.75f, 0f), Quaternion.identity);
                droppedMoney.GetComponent<MoneyDrop>().money = dropMoney;
                GameObject droppedAmmo = Instantiate(ammoDropPrefab, transform.position + new Vector3(0.75f, -0.75f, 0f), Quaternion.identity);
                GameObject droppedMedkit = Instantiate(medkitPrefab, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
                droppedMedkit.GetComponent<Medkit>().hp = dropHP;
                currentTimeBloodPool = Time.time;
            }
        }
        healthBar.GetComponent<EnemyHealthBar>().SetHealth(currentHealth);
        currentTime = Time.time;
    }
}
