using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public int experiencePoints;
    public int money;
    public int equippedWeaponID;
    public string equippedWeaponName;

    public HealthBar healthBar;
    public Bullet bullet;

    public Rigidbody2D rb;

    float currentTime;

    Vector2 moveDirection;
    Vector2 mousePosition;

    // KK: Dodaje prefab z canvasem i pauzę do levela w którym jest gracz.
    private void Start()
    {
        //GameObject pause = Instantiate(pausePrefab);
        currentHealth = maxHealth;
        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        money = PlayerPrefs.GetInt("money");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        equippedWeaponName = PlayerPrefs.GetString("equippedWeaponName");
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);
        healthBar.SetWeapon(equippedWeaponName);
    }

    //KK: Prosto z poradnika Brackeys (RIP).
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //KK: Wciskamy spację, zabiera 20 HP od nas.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(15);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetXP(15);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GetMoney(1000);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.DeleteKey("experiencePoints");
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 84f;
        rb.rotation = aimAngle;
    }


    //KK: Kiedy dostajemy od czegoś wpierdol, usuwa damage od obecnego zdrowia gracza i ustawia wartość filla w healthBarze na obecne HP gracza.
    //Tutaj zrób skrypt na game over.
    public void TakeDamage(int damage)
    {
        if (Time.time - currentTime > .01f)
        {
            currentHealth -= damage;
            currentTime = Time.time;
        }
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void GetXP(int XP)
    {
        experiencePoints += XP;
        if (experiencePoints < 0) experiencePoints = 0;
        healthBar.SetExperiencePoints(experiencePoints);
        PlayerPrefs.SetInt("experiencePoints", experiencePoints);
    }

    public void GetMoney(int gotMoney)
    {
        money += gotMoney;
        if (money < 0) money = 0;
        healthBar.SetMoney(money);
        PlayerPrefs.SetInt("money", money);
    }
}