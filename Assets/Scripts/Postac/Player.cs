using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    public Rigidbody2D rb;
    public GameObject pausePrefab;

    Vector2 moveDirection;
    Vector2 mousePosition;

    // KK: Dodaje prefab z canvasem i pauza do levela w którym jest gracz.
    private void Start()
    {
        GameObject pause = Instantiate(pausePrefab);
    }

    //KK: Prosto z poradnika Brackeys (RIP).
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 87f;
        rb.rotation = aimAngle;
    }
}