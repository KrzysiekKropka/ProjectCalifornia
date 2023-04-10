using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite cursorYellow, cursorRed;
    float crosshairReach = 1f;

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


            //RaycastHit2D rightRay = Physics2D.Raycast(transform.position + new Vector3(-crosshairReach*1.5f, 0, 0), transform.right);
            //RaycastHit2D upRay = Physics2D.Raycast(transform.position + new Vector3(0, -crosshairReach, 0), transform.up);

            /*if (rightRay.collider != null && rightRay.distance < crosshairReach)
            {
                if (rightRay.collider.tag == "Enemy")
                {
                    print("RightRay" + rightRay.distance);
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

            if (upRay.collider != null && upRay.distance < crosshairReach)
            {
                if (upRay.collider.tag == "Enemy")
                {
                    print("UpRay" + upRay.distance);
                    spriteRenderer.sprite = cursorRed;
                }
                else
                {
                    spriteRenderer.sprite = cursorYellow;
                }
            }*/
        }
        else
        {
            Cursor.visible = true;
            spriteRenderer.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
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
