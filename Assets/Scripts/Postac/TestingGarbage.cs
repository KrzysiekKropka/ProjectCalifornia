using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGarbage : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public GameObject player;

    public Vector2 movementInput;

    void FixedUpdate()
    {

        ///KK: Ten kod jest kurwa autystyczny, w chuj work-in-progress
        Vector2 direction = new Vector2(transform.up.x, transform.up.y);
        if (Input.GetKey(KeyCode.W))
        {
            speed = 50f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speed = -50f;
        }
        else
        {
            speed = 0f;
        }
        rb.velocity = direction * Time.fixedDeltaTime * speed;
        if (rb.velocity.magnitude < .01)
        {
            rb.velocity = Vector3.zero;
        }
        Debug.Log(rb.velocity.magnitude);
    }
}