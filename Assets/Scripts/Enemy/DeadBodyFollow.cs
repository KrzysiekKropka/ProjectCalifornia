using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyFollow : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipY = true;
    }

    public void DeadBodyPosition(int bodyColor)
    {
        spriteRenderer.sprite = sprites[bodyColor];

        transform.position = enemy.transform.position;
        transform.rotation = enemy.transform.rotation;
        if (Random.Range(0, 2) == 1) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
