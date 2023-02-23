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

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - currentTime > .01f)
        {
            currentHealth -= damage;
            currentTime = Time.time;
        }
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            GameObject effect = Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}
