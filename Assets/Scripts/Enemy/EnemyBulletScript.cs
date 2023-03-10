using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBulletScript : MonoBehaviour
{
    public GameObject genericEffect;
    public GameObject bloodEffect;
    GameObject effect;
    public int bulletDamage;
    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        var enemy = collision.collider.GetComponent<Player>();
        var tilemap = collision.collider.GetComponent<TilemapCollider2D>();
        var bullet = collision.collider.GetComponent<PlayerBullet>();

        if (player)
        {
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (tilemap)
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (bullet)
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            bulletDamage /= 2;
        }
        else if (enemy && Time.time - startTime > 0.01f)
        {
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(bulletDamage);
        }
        else
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        Destroy(effect, 1f);
    }
}
