using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject genericEffect;
    public GameObject bloodEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.collider.GetComponent<AIBrain>();
        if (enemy)
        {
            enemy.TakeDamage(20);
            GameObject effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
        else
        {
            ///GameObject effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
