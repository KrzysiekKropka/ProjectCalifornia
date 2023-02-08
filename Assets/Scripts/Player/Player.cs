using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class Player : MonoBehaviour
{
    [DllImport("user32.dll")] static extern bool SetCursorPos(int X, int Y);

    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public int experiencePoints;

    public HealthBar healthBar;

    public Rigidbody2D rb;
    public GameObject pausePrefab;

    Vector2 moveDirection;
    Vector2 mousePosition;

    // KK: Dodaje prefab z canvasem i pauzę do levela w którym jest gracz.
    private void Start()
    {
        GameObject pause = Instantiate(pausePrefab);
        currentHealth = maxHealth;
        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetExperiencePoints(experiencePoints);
        PlayerPrefs.SetInt("money", 10000);
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.DeleteKey("experiencePoints");
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 87f;
        rb.rotation = aimAngle;
    }


    //KK: Kiedy dostajemy od czegoś wpierdol, usuwa damage od obecnego zdrowia gracza i ustawia wartość filla w healthBarze na obecne HP gracza.
    //Tutaj zrób skrypt na game over.
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    void GetXP(int XP)
    {
        experiencePoints += XP;
        healthBar.SetExperiencePoints(experiencePoints);
        PlayerPrefs.SetInt("experiencePoints", experiencePoints);
    }
}