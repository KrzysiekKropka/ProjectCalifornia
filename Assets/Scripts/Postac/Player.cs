using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;

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
        healthBar.SetMaxHealth(maxHealth);
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
            TakeDamage(20);
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
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
        if(currentHealth == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}