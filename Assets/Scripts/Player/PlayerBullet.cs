using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] GameObject genericEffect;
    [SerializeField] GameObject bloodEffect;
    private GameObject effect;

    public int bulletDamage;
    float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<AIBrain>();
        var player = collision.collider.GetComponent<Player>();
        var tilemap = collision.collider.GetComponent<TilemapCollider2D>();
        var bullet = collision.collider.GetComponent<AIBullet>();

        if (enemy)
        {
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(bulletDamage);
        }
        else if (tilemap)
        {
            Destroy(gameObject);
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }
        else if (bullet)
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            bulletDamage /= 2;
        }
        else if (player && Time.time - startTime > 0.01f)
        {
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.TakeDamage(bulletDamage);
        }
        else
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        Destroy(effect, 1f);
    }
}
