using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Objects
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private NextLevelScreen nextLevelScreen;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject triggerNextLevelMenu;
    [SerializeField] private GameObject ShopManager;
    [SerializeField] private AudioClip manHurtClip, healClip, dashClip;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private GameObject damagePopup;

    //Global Bools
    public bool isSprinting;
    public bool isDashing;
    public bool isInvincible;
    public static bool inInventory = false;

    //Store Bools
    public bool canBetterAim;
    private bool canSprint;
    private bool canDash;
    private bool canMoreHP;

    //Statistics
    public int kills = 0;
    public int enemies = 0;
    public int remainingEnemies = 0;
    public int experiencePoints;
    public int money;

    //Stamina Related
    private bool inStaminaCooldown;
    private float staminaCooldown = 1f;
    private float maxStamina = 60f;
    private float remainingStamina;

    //Dashing Related
    private bool inDashingCooldown;
    private float dashingSpeed = 24f;
    private float dashingTime = 0.1f;
    private float dashingTimer;
    private float dashingCooldown = 0.25f;
    private float invincibiltyTimer = 0.375f;
    private int dashingSpree;

    //Movement Related
    private float speed = 10f;
    private float sprintingSpeed = 12.5f;

    //Health
    private int maxHealth = 100;
    private int currentHealth;

    //Weapon Related
    private int equippedWeaponID;
    private int summedDamage;

    //Movement Data
    private float aimAngle;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private Vector2 aimDirection;
    private Vector2 dashDirection;

    //IEnumerators for storing coroutines
    private IEnumerator stopInvincibilityCoroutine, dashingCooldownCoroutine;

    void Start()
    {
        NextLevelScreen.isActive = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        money = PlayerPrefs.GetInt("money");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        healthBar.SetKills(kills);

        RefreshShop();

        currentHealth = maxHealth;
        remainingStamina = maxStamina;

        healthBar.SetHealth(currentHealth);
        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);
        if(!GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isArena) healthBar.MessageBox("Kill them all!");
        //else healthBar.MessageBox("Change the level by pressing minus or plus key!");
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

        if (canSprint || canDash) healthBar.SetMaxStamina(maxStamina);
        else healthBar.HideStamina();

        healthBar.SetExperiencePoints(experiencePoints);
        healthBar.SetMoney(money);
    }

    //KK: Prosto z poradnika Brackeys (RIP).
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            healthBar.gameObject.SetActive(!healthBar.gameObject.activeSelf);
        }

        if (Input.GetKey(KeyCode.LeftShift) && canSprint && !inStaminaCooldown && rb.velocity != Vector2.zero)
        {
            isSprinting = true;
        }
        else if (isSprinting) isSprinting = false;

        if (remainingStamina < maxStamina)
        {
            if (!isSprinting && !isDashing && !inDashingCooldown)
            {
                remainingStamina += 10f * Time.deltaTime;
            }
            healthBar.SetStamina(remainingStamina);
        }
    }

    void FixedUpdate()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = mousePosition - rb.position;
        aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

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

    void Dash()
    {
        if(canDash && !inStaminaCooldown && !inDashingCooldown && !isDashing && rb.velocity != Vector2.zero && !NextLevelScreen.isActive)
        {
            if(stopInvincibilityCoroutine != null) StopCoroutine(stopInvincibilityCoroutine);
            if (dashingCooldownCoroutine != null) StopCoroutine(dashingCooldownCoroutine);
            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            float previousDashingTimer = dashingTimer;
            dashingTimer = Time.time;

            //KK: Ten kod jest rakiem.

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
                if(dashingSpree >= 2)
                {
                    dashingSpree = 0;
                    dashingCooldownCoroutine = DashingCooldown();
                    StartCoroutine(dashingCooldownCoroutine);
                }
            }
            else
            {
                dashingSpree = 0;
                dashingCooldownCoroutine = DashingCooldown();
                StartCoroutine(dashingCooldownCoroutine);
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
            AudioSource.PlayClipAtPoint(manHurtClip, transform.position, 1f);
            currentHealth -= damage;

            if (currentHealth > 0) healthBar.SetHealth(currentHealth);
            else SceneManager.LoadScene("GameOver");
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
        remainingEnemies--;
        if (kills >= enemies)
        {
            if (!GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isArena && !GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isTheEnd && !GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isArena)
            {
                GameObject.FindWithTag("NextLevelTrigger").transform.GetChild(0).GetComponent<BoxCollider2D>().isTrigger = true;
                healthBar.MessageBox("You killed them all!\nGo on to the next level!");
            }
            else if(GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().isTheEnd)
            {
                StartCoroutine(TriggerTheEnd());
            }
        }
        healthBar.SetKills(kills);
    }

    public void AddEnemy()
    {
        enemies++;
        remainingEnemies++;
    }

    public void SetXP(int XP)
    {
        if (experiencePoints < 0) experiencePoints = 0;
        experiencePoints += XP;
        healthBar.SetExperiencePoints(experiencePoints);
        PlayerPrefs.SetInt("experiencePoints", experiencePoints);
    }

    public void SetMoney(int gotMoney)
    {
        if (money < 0) money = 0;
        money += gotMoney;
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
        inStaminaCooldown = true;
        yield return new WaitForSeconds(staminaCooldown);
        inStaminaCooldown = false;
    }

    IEnumerator TriggerTheEnd()
    {
        NextLevelScreen.isActive = true;
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(15);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }
}