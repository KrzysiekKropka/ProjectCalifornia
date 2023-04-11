using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite cursorYellow, cursorRed;
    Collider2D[] Colliders;

    void Start()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cursorYellow;
    }

    void Update()
    {
        Colliders = Physics2D.OverlapCircleAll(transform.position, 0.25f);
        if (Colliders.Length > 0)
        {
            spriteRenderer.sprite = cursorYellow;
            foreach (Collider2D Enemy in Colliders)
            {
                if (Enemy.gameObject.tag == "Enemy" && !Enemy.isTrigger)
                {
                    spriteRenderer.sprite = cursorRed;
                }
            }
        }
        else
        {
            spriteRenderer.sprite = cursorYellow;
        }

        if (!PauseMenu.isPaused && !Player.inInventory)
        {
            Cursor.visible = false;
            spriteRenderer.enabled = true;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;
        }
        else
        {
            Cursor.visible = true;
            spriteRenderer.enabled = false;
        }
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        if (collision.CompareTag("Enemy") && !collision.isTrigger)
        {
            spriteRenderer.sprite = cursorRed;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        if (collision.CompareTag("Enemy") && collision.isTrigger) 
            spriteRenderer.sprite = cursorYellow;
    }*/
}
