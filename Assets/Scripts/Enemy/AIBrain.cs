using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBrain : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] GameObject bloodPoolEffect;
    [SerializeField] Rigidbody2D rb;
    private GameObject player;

    bool readyToShoot = false;
    int maxHealth = 100;
    int currentHealth;
    public int dropXP;
    public int dropMoney;
    float currentTime;
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
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 87f;
    }

    void FixedUpdate()
    {
        rb.rotation = aimAngle;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - currentTime > .01f)
        {
            currentTime = Time.time;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                if (player)
                {
                    player.SetXP(dropXP);
                    player.AddKill();
                }
                Destroy(transform.parent.gameObject);
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
            }
            healthBar.SetHealth(currentHealth);
        }
    }
}
