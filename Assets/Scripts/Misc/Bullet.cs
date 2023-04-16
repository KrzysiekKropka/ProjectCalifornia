using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject genericEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] AudioClip ricochetClip, bulletHitClip;
    private AudioClip bodyHitClip;
    private GameObject effect;

    bool enteredEnemy = false;
    bool initiatedStart = false;
    public int? weaponID;
    public int bulletDamage;
    int modifiedBulletDamage;
    public bool playerIsOwner = false;
    public bool enemyIsOwner = false;
    float startTime;
    float playerYPosition;

    void Start()
    {
        PlaySound();
        initiatedStart = true;
        int randomInt2 = Random.Range(1, 17);
        AudioClip passByClip = (AudioClip)Resources.Load("Audio/PassBy" + randomInt2);
        AudioSource.PlayClipAtPoint(passByClip, transform.position, 1f);
        startTime = Time.time;
        if (playerIsOwner) 
        {
            gameObject.tag = "PlayerBullet";
            playerYPosition = GameObject.FindWithTag("Player").transform.position.y;
            //gameObject.layer = 8;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        modifiedBulletDamage = bulletDamage;

        if (!initiatedStart) 
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
            player.TakeDamage(modifiedBulletDamage);
            Destroy(gameObject);
        }
        else if (tilemap)
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            int randomInt = Random.Range(1, 4);
            ricochetClip = (AudioClip)Resources.Load("Audio/HitGeneric" + randomInt);
            AudioSource.PlayClipAtPoint(ricochetClip, transform.position, 1f);
            if (playerIsOwner && playerYPosition > transform.position.y)
            {
                effect.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else if (playerIsOwner && playerYPosition < transform.position.y)
            {
                effect.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            Destroy(gameObject);
        }
        else if (bullet)
        {
            int randomInt = Random.Range(1, 5);
            bulletHitClip = (AudioClip)Resources.Load("Audio/HitBullet" + randomInt);
            AudioSource.PlayClipAtPoint(bulletHitClip, transform.position, 1f);
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //modifiedBulletDamage = bulletDamage / 2;
            //enemyIsOwner = false;
            //playerIsOwner = false;
        }
        else if(playerIsOwner && enemy && enteredEnemy == false)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody" + randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
            enteredEnemy = true;
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(modifiedBulletDamage);
            enemy.PlayerInterrupts();
        }
        else if (enemy && enteredEnemy == false)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody" + randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
            enteredEnemy = true;
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(modifiedBulletDamage);
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
        // ja pierdole pozdrawiam ludzi ktorzy beda musieli to czytac w przyszlosci zawiodlem was
        if(weaponID != null)AudioSource.PlayClipAtPoint(GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>().weaponAudioClips[weaponID.Value].clips[Random.Range(0, GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>().weaponAudioClips[weaponID.Value].clips.Length)], transform.position, 1f);
    }
}