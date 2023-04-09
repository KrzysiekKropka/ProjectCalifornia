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
        if(!PauseMenu.isPaused && !Player.inInventory)
        {
            Cursor.visible = false;
            spriteRenderer.enabled = true;
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;

            RaycastHit2D rightRay = Physics2D.Raycast(transform.position + new Vector3(-2, 0, 0), transform.right);

            if (rightRay.collider != null && rightRay.distance < 2f)
            {
                if (rightRay.collider.tag == "Enemy")
                {
                    spriteRenderer.sprite = cursorRed;
                }
                else
                {
                    spriteRenderer.sprite = cursorYellow;
                }
            }
            else
            {
                spriteRenderer.sprite = cursorYellow;
            }

            RaycastHit2D upRay = Physics2D.Raycast(transform.position + new Vector3(0, -2, 0), transform.up);

            if (upRay.collider != null && upRay.distance < 2f)
            {
                if (upRay.collider.tag == "Enemy")
                {
                    spriteRenderer.sprite = cursorRed;
                }
                else
                {
                    spriteRenderer.sprite = cursorYellow;
                }
            }
            else
            {
                spriteRenderer.sprite = cursorYellow;
            }
            //Debug.DrawRay(transform.position + new Vector3(1f, 0, 0), transform.right, Color.red);
        }
        else
        {
            Cursor.visible = true;
            spriteRenderer.enabled = false;
        }
    }
}
