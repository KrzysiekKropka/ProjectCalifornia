using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBrain : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public EnemyHealthBar healthBar;

    float currentTime;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - currentTime > .1F)
        {
            currentHealth -= damage;
            currentTime = Time.time;
        }

        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
