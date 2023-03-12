using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject genericEffect;
    [SerializeField] GameObject bloodEffect;
    private GameObject effect;

    bool enteredEnemy = false;
    int originalBulletDamage;
    public int bulletDamage;
    float startTime;

    void Start()
    {
        startTime = Time.time;
        originalBulletDamage = bulletDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        var enemy = collision.collider.GetComponent<AIBrain>();
        var tilemap = collision.collider.GetComponent<TilemapCollider2D>();
        var bullet = collision.collider.GetComponent<Bullet>();

        if (player && Time.time - startTime > 0.01f)
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
            bulletDamage = originalBulletDamage / 2;
        }
        else if (enemy && enteredEnemy == false)
        {
            enteredEnemy = true;
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
