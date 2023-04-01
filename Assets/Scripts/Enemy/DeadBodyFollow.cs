using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyFollow : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipY = true;
        DeadBodyPosition();
    }

    void DeadBodyPosition()
    {
        transform.position = enemy.transform.position;
        transform.rotation = enemy.transform.rotation;
        if (Random.Range(0, 2) == 1) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
