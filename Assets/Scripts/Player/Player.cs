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
    [SerializeField] GameObject triggerNextLevelMenu;
    [SerializeField] GameObject ShopManager;
    [SerializeField] AudioClip manHurtClip, healClip, dashClip;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private GameObject damagePopup;

    public static bool inInventory = false;
    public int kills = 0;
    public int enemies = 0;

    bool inFocus;
    public bool isSprinting;
    public bool isDashing;
    public bool isInvincible;

    bool staminaCooldownBool;
    float staminaCooldown = 3f;
    float maxStamina = 60f;
    float remainingStamina;

    bool inDashingCooldown;
    float dashingSpeed = 24f;
    float dashingTime = 0.1f;
    float dashingTimer;
    float dashingCooldown = 0.25f;
    float invincibiltyTimer = 0.375f;
    int dashingSpree;

    float speed = 9f;
    float sprintingSpeed = 14f;

    int maxHealth = 100;
    int currentHealth;

    int equippedWeaponID;
    public int experiencePoints;
    public int money;
    int summedDamage;
    string equippedWeaponName;

    float aimAngle;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private Vector2 aimDirection;
    private Vector2 dashDirection;

    private bool canSprint;
    private bool canDash;
    public bool canBetterAim;
    private bool canMoreHP;

    private IEnumerator stopInvincibilityCoroutine;

    float currentTime;

    void Start()
    {
        NextLevelScreen.isActive = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        money = PlayerPrefs.GetInt("money");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        equippedWeaponName = PlayerPrefs.GetString("equippedWeaponName");
        healthBar.SetKills(kills);

        RefreshShop();

        currentHealth = maxHealth;

        healthBar.SetHealth(currentHealth);
        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);

        healthBar.MessageBox("Kill them all!");
    }

    public void RefreshShop()
    {
        canSprint = ShopManager.GetComponent<ShopManager>().shopSkills[3, 1] == 1;
        canDash = ShopManager.GetComponent<ShopManager>().shopSkills[3, 2] == 1;
        canBetterAim = ShopManager.GetComponent<ShopManager>().shopSkills[3, 3] == 1;
        canMoreHP = ShopManager.GetComponent<ShopManager>().shopSkills[3, 4] == 1;

        if (canMoreHP) maxHealth = 150;
        else maxHealth = 100;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        if (canSprint || canDash) remainingStamina = maxStamina;

        if (canSprint || canDash) healthBar.SetMaxStamina(maxStamina);
        else healthBar.HideStamina();

        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);
    }

    //KK: Prosto z poradnika Brackeys (RIP).
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        /* if (Input.GetKeyDown(KeyCode.Q))
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
        } */

        if(Input.GetKey(KeyCode.LeftShift) && canSprint && !staminaCooldownBool && rb.velocity != Vector2.zero)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (remainingStamina < maxStamina)//remainingStamina < maxStamina
        {
            if (!isSprinting && !isDashing)
            {
                remainingStamina += 10f * Time.deltaTime;
            }
            healthBar.SetStamina(remainingStamina);
        }

    }

    void FixedUpdate()
    {
        if (isSprinting && remainingStamina > 0)
        {
            rb.velocity = new Vector2(moveDirection.x * sprintingSpeed, moveDirection.y * sprintingSpeed);
            if (!isDashing) remainingStamina -= 1f * Time.deltaTime;
            if (remainingStamina <= 0) StartCoroutine(SprintCooldown());
        }
        else
        {
            rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        }

        if (isDashing) rb.velocity = new Vector2(dashDirection.x * dashingSpeed, dashDirection.y * dashingSpeed);
        if (inInventory == false) rb.rotation = aimAngle;
    }

    /*void LateUpdate()
    {
        healthBar.SetStamina(remainingStamina);
    }*/

    void Dash()
    {
        if(canDash && !staminaCooldownBool && !inDashingCooldown && !isDashing && rb.velocity != Vector2.zero && !NextLevelScreen.isActive)
        {
            if(stopInvincibilityCoroutine != null) StopCoroutine(stopInvincibilityCoroutine);
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            float previousDashingTimer = dashingTimer;
            dashingTimer = Time.time;

            if (dashingTimer - previousDashingTimer > dashingCooldown)
            {
                dashingSpree = 0;
                isDashing = true;
                isInvincible = true;
                trailRenderer.emitting = true;
                remainingStamina -= 2f;
                AudioSource.PlayClipAtPoint(dashClip, transform.position, 1f);
            }
            else if (dashingSpree < 2)
            {
                dashingSpree++;
                isDashing = true;
                isInvincible = true;
                trailRenderer.emitting = true;
                remainingStamina -= 2f;
                AudioSource.PlayClipAtPoint(dashClip, transform.position, 1f);
            }
            else
            {
                dashingSpree = 0;
                StartCoroutine(DashingCooldown());
            }
            if (remainingStamina <= 0) StartCoroutine(SprintCooldown());
            StartCoroutine(StopDashing());
            stopInvincibilityCoroutine = StopInvincibility();
            StartCoroutine(stopInvincibilityCoroutine);
        }
    }

    //KK: Kiedy dostajemy od czegoś wpierdol, usuwa damage od obecnego zdrowia gracza i ustawia wartość filla w healthBarze na obecne HP gracza.
    //Tutaj zrób skrypt na game over.
    public void TakeDamage(int damage)
    {
        if(!isInvincible)
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
        if (kills >= enemies)
        {
            if (!GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isArena && !GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isTheEnd)
            {
                GameObject.FindWithTag("NextLevelTrigger").GetComponent<BoxCollider2D>().isTrigger = true;
                healthBar.MessageBox("You killed them all!\nGo on to the next level!");
            }
            else if(GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isTheEnd)
            {
                GameObject.FindWithTag("NextLevelTrigger").GetComponent<BoxCollider2D>().isTrigger = true;
                healthBar.MessageBox("Congratulations on finishing the game!");
                StartCoroutine(EndTheGame());
            }
        }
        healthBar.SetKills(kills);
    }

    public void AddEnemy()
    {
        enemies++;
        //healthBar.SetKills(kills);
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

    public void TriggerNextLevel()
    {
        if(!GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isTheEnd)
        {
            triggerNextLevelMenu.SetActive(true);
            triggerNextLevelMenu.GetComponent<NextLevelScreen>().StartCountdown();
        }
    }

    IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        trailRenderer.emitting = false;
    }

    IEnumerator StopInvincibility()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincibiltyTimer);
        isInvincible = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    IEnumerator DashingCooldown()
    {
        inDashingCooldown = true;
        yield return new WaitForSeconds(dashingCooldown);
        inDashingCooldown = false;
    }

    IEnumerator SprintCooldown()
    {
        staminaCooldownBool = true;
        yield return new WaitForSeconds(staminaCooldown);
        staminaCooldownBool = false;
    }

    IEnumerator EndTheGame()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Menu");
    }
}