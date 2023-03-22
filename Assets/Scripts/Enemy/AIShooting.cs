using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    private GameObject player;

    [SerializeField] int weaponDamage = 10;
    [SerializeField] int weaponDelay = 2;
    float bulletForce = 35f;
    float distance;
    float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        timer += Time.deltaTime;
        if (timer > weaponDelay && distance < 15f)
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje nab�j z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
        bullet.GetComponent<Bullet>().bulletDamage = weaponDamage;
        Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
        GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
    }
}
