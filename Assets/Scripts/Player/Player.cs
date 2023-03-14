using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] NextLevelScreen nextLevelScreen;
    [SerializeField] Rigidbody2D rb;

    public static bool inInventory = false;

    float speed = 7.5f;
    float aimAngle;
    float currentTime;
    int maxHealth = 100;
    int currentHealth;
    int equippedWeaponID;
    int kills = 0;
    int experiencePoints;
    int money;
    string equippedWeaponName;

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private Vector2 aimDirection;

    void Start()
    {
        currentHealth = maxHealth;
        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        money = PlayerPrefs.GetInt("money");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        equippedWeaponName = PlayerPrefs.GetString("equippedWeaponName");
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);
        healthBar.SetKills(kills);
    }

    //KK: Prosto z poradnika Brackeys (RIP).
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetXP(15);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SetMoney(1000);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(10);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        if (inInventory == false) rb.rotation = aimAngle;
    }

    //KK: Kiedy dostajemy od czegoś wpierdol, usuwa damage od obecnego zdrowia gracza i ustawia wartość filla w healthBarze na obecne HP gracza.
    //Tutaj zrób skrypt na game over.
    public void TakeDamage(int damage)
    {
        if (Time.time - currentTime > .001f)
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

    public void AddKill()
    {
        kills++;
        healthBar.SetKills(kills);
    }

    public void SetXP(int XP)
    {
        experiencePoints += XP;
        if (experiencePoints < 0) experiencePoints = 0;
        healthBar.SetExperiencePoints(experiencePoints);
        PlayerPrefs.SetInt("experiencePoints", experiencePoints);
    }

    public void SetMoney(int gotMoney)
    {
        money += gotMoney;
        if (money < 0) money = 0;
        healthBar.SetMoney(money);
        PlayerPrefs.SetInt("money", money);
    }
}