using UnityEngine;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviour
{
    //Objects
    [SerializeField] GameObject genericEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] GameObject blockEffect;
    private AudioClip bodyHitClip, ricochetClip, bulletHitClip;
    private GameObject effect;

    //Bools
    bool enteredEnemy = false;
    bool initiatedStart = false;
    public bool playerIsOwner = false;
    public bool enemyIsOwner = false;

    //Ints
    public int? weaponID;
    public int bulletDamage;

    //Floats
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
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!initiatedStart) 
        {
            PlaySound();
        }

        var player = collision.collider.GetComponent<Player>();
        var enemy = collision.collider.GetComponent<AIBrain>();
        var tilemap = collision.collider.GetComponent<TilemapCollider2D>();
        var bullet = collision.collider.GetComponent<Bullet>();


        if (enemyIsOwner && player && !player.isInvincible)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody" + randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
        else if (enemyIsOwner && player && player.isInvincible)
        {
            effect = Instantiate(blockEffect, transform.position, Quaternion.identity);
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
                effect.gameObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            else if (playerIsOwner && playerYPosition < transform.position.y)
            {
                effect.gameObject.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
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
        }
        else if(playerIsOwner && enemy && enteredEnemy == false)
        {
            int randomInt = Random.Range(1, 5);
            bodyHitClip = (AudioClip)Resources.Load("Audio/HitBody" + randomInt);
            AudioSource.PlayClipAtPoint(bodyHitClip, transform.position, 1f);
            enteredEnemy = true;
            Destroy(gameObject);
            effect = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            enemy.TakeDamage(bulletDamage);
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
            enemy.TakeDamage(bulletDamage);
        }
        else
        {
            effect = Instantiate(genericEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void PlaySound()
    {
        Shooting shooting = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooting>();
        // ja pierdole pozdrawiam ludzi ktorzy beda musieli to czytac w przyszlosci zawiodlem was
        if(weaponID != null)AudioSource.PlayClipAtPoint(shooting.weaponAudioClips[weaponID.Value].clips[Random.Range(0, shooting.weaponAudioClips[weaponID.Value].clips.Length)], transform.position, 0.75f);
    }
}
