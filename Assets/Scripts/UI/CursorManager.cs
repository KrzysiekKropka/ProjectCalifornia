using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite cursorYellow, cursorRed;

    void Start()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cursorYellow;
    }

    void Update()
    {
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger && collision.CompareTag("Enemy"))
        {
            spriteRenderer.sprite = cursorRed;
        }
        else spriteRenderer.sprite = cursorYellow;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            spriteRenderer.sprite = cursorYellow;
        }
    }
}
