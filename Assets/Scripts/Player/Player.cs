using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] NextLevelScreen nextLevelScreen;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] AudioClip manHurtClip, healClip, dashClip;
    private TrailRenderer trailRenderer;
    private GameObject damagePopup;

    public static bool inInventory = false;

    bool inFocus;
    bool isDashing;
    bool inDashingCooldown;
    float dashingSpeed = 30f;
    float dashingTime = 0.075f;
    float dashingTimer;
    float dashingCooldown = 0.33f;
    float speed = 6f;
    float aimAngle;
    float currentTime;
    int dashingSpree;
    int maxHealth = 100;
    int currentHealth;
    int equippedWeaponID;
    int kills = 0;
    int experiencePoints;
    int money;
    int summedDamage;
    string equippedWeaponName;

    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private Vector2 aimDirection;
    private Vector2 dashDirection;

    private bool canMoreHP;
    private bool canMoreSpeed;
    private bool canDash;
    //private bool canFocus;

    void OnEnable()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        canMoreSpeed = PlayerPrefs.GetInt("skillQuantity" + 1) >= 1;
        canMoreHP = PlayerPrefs.GetInt("skillQuantity" + 2) >= 1;
        canDash = PlayerPrefs.GetInt("skillQuantity" + 3) >= 1;
        //canFocus = PlayerPrefs.GetInt("skillQuantity" + 4) >= 1;

        if (canMoreHP) maxHealth = 150;
        else maxHealth = 100;

        if (canMoreSpeed) speed = 8f;
        else speed = 6f;

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
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        if (Input.GetKeyDown(KeyCode.Space) && canDash && !inDashingCooldown && !isDashing)
        {
            Dash();
        }

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

        /*if (Input.GetKeyDown(KeyCode.LeftShift) && canFocus)
        {
            if(!inFocus)
            {
                Time.timeScale = 0.5f;
                speed *= 2f;
                inFocus = true;
            }
            else if (inFocus)
            {
                Time.timeScale = 1f;
                speed /= 2f;
                inFocus = false;
            }
        }*/
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        if (isDashing) rb.velocity = new Vector2(dashDirection.x * dashingSpeed, dashDirection.y * dashingSpeed);
        if (inInventory == false) rb.rotation = aimAngle;
    }

    void Dash()
    {
        float previousDashingTimer = dashingTimer;
        dashingTimer = Time.time; 
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (dashingTimer - previousDashingTimer > dashingCooldown)
        {
            dashingSpree = 0;
            isDashing = true;
            trailRenderer.emitting = true;
            AudioSource.PlayClipAtPoint(dashClip, transform.position, 1f);
        }
        else if (dashingSpree < 2)
        {
            dashingSpree++;
            isDashing = true;
            trailRenderer.emitting = true;
            AudioSource.PlayClipAtPoint(dashClip, transform.position, 1f);
        }
        else
        {
            dashingSpree = 0;
            StartCoroutine(DashingCooldown());
        }
        StartCoroutine(StopDashing());
    }

    //KK: Kiedy dostajemy od czegoś wpierdol, usuwa damage od obecnego zdrowia gracza i ustawia wartość filla w healthBarze na obecne HP gracza.
    //Tutaj zrób skrypt na game over.
    public void TakeDamage(int damage)
    {
        AudioSource.PlayClipAtPoint(manHurtClip, transform.position, 0.2f);
        currentHealth -= damage;
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
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public bool AddHP(int hp)
    {
        if (currentHealth == maxHealth)
        {
            return false;
        }
        else if (currentHealth + hp <= maxHealth)
        {
            currentHealth += hp;
            healthBar.SetHealth(currentHealth);
            AudioSource.PlayClipAtPoint(healClip, transform.position, 1f);
            return true;
        }
        else if (currentHealth + hp > maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
            AudioSource.PlayClipAtPoint(healClip, transform.position, 1f);
            return true;
        }
        else
        {
            return false;
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

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        trailRenderer.emitting = false;
    }

    IEnumerator DashingCooldown()
    {
        inDashingCooldown = true;
        yield return new WaitForSeconds(dashingCooldown);
        inDashingCooldown = false;
    }
}