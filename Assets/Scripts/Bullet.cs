using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject genericEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] AudioSource audioData;
    [SerializeField] AudioClip ricochetClip, bulletHitClip, bodyHitClip;
    private GameObject effect;

    bool enteredEnemy = false;
    bool initiatedStart = false;
    public string weaponName;
    public int bulletDamage;
    public bool playerIsOwner = false;
    public bool enemyIsOwner = false;
    float startTime;

    void Start()
    {
        if (weaponName != "")
        {
            PlaySound();
        }
        initiatedStart = true;
        int randomInt2 = Random.Range(1, 17);
        AudioClip passByClip = (AudioClip)Resources.Load("Audio/PassBy" + randomInt2);
        AudioSource.PlayClipAtPoint(passByClip, transform.position, 1f);
        startTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!initiatedStart && weaponName != "") 
        {
            PlaySound();
        }

        var player = collision.collider.GetComponent<Player>();
        var enemy = collision.collider.GetComponent<AIBrain>();
        var tilemap = collision.collider.GetComponent<TilemapCollider2D>();
        var bullet = collision.collider.GetComponent<Bullet>();

        if (!playerIsOwner && player)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody" + randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (tilemap)
        {
            int randomInt = Random.Range(1, 5);
            ricochetClip = (AudioClip)Resources.Load("Audio/HitMetal" + randomInt);
            AudioSource.PlayClipAtPoint(ricochetClip, transform.position, 1f);
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (bullet)
        {
            AudioSource.PlayClipAtPoint(bulletHitClip, transform.position, 0.5f);
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            bulletDamage /= 2;
            enemyIsOwner = false;
            playerIsOwner = false;
        }
        else if (!enemyIsOwner && enemy && enteredEnemy == false)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody"+randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
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

    void PlaySound()
    {
        int randomInt = Random.Range(1, 4);
        AudioClip weaponShootClip = (AudioClip)Resources.Load("Audio/Weapons/" + weaponName + "/Shoot" + randomInt);
        AudioSource.PlayClipAtPoint(weaponShootClip, transform.position, 0.66f);
    }
}
