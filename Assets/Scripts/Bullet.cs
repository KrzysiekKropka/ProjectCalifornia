using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject genericEffect;
    public GameObject bloodEffect;
    GameObject effect;
    public int bulletDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        var enemy = collision.collider.GetComponent<AIBrain>();
        var player = collision.collider.GetComponent<Player>();

        if (enemy)
        {
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(bulletDamage);
        }
        else if (player)
        {
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.TakeDamage(bulletDamage);
            player.SetXP(-15);
        }
        else
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }
        Destroy(effect, 1f);
    }
}
