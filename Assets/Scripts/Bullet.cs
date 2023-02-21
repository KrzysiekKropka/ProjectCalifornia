using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject genericEffect;
    public GameObject bloodEffect;
    GameObject effect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<AIBrain>();
        var player = collision.collider.GetComponent<Player>();

        if (enemy)
        {
            enemy.TakeDamage(20);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }
        else if (player)
        {
            player.TakeDamage(20);
            player.GetXP(-15);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }
        else
        {
            ///effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }

        Destroy(effect, 1f);
        Destroy(gameObject);
    }
}
