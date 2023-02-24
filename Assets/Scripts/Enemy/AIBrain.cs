using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBrain : MonoBehaviour
{
    int maxHealth = 100;
    int currentHealth;

    public EnemyHealthBar healthBar;
    public GameObject bloodPoolEffect;
    float currentTime;

    public int dropXP;
    public int dropMoney;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
