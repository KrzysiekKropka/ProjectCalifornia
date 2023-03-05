using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBullet : MonoBehaviour
{
    public GameObject genericEffect;
    public GameObject bloodEffect;
    GameObject effect;
    public int bulletDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        var enemy = collision.collider.GetComponent<AIBrain>();
        var decoration = collision.collider.GetComponent<TilemapCollider2D>();

        if (enemy)
        {
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(bulletDamage);
        }
        else if (decoration)
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }
        else
        {
            //effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }
        Destroy(effect, 1f);
    }
}
