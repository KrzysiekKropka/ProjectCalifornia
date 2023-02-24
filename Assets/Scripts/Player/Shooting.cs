using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    int equippedWeaponID;
    int weaponDamage;
    float weaponDelay;
    float bulletForce = 30f;
    float currentTime;

    void Start()
    {
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");

        switch (equippedWeaponID)
        {
            case 1:
                weaponDelay = 0.75f;
                weaponDamage = 25;
                break;
            case 2:
                weaponDelay = 0.1f;
                weaponDamage = 10;
                break;
            case 3:
                weaponDelay = 1f;
                weaponDamage = 35;
                break;
            case 4:
                weaponDelay = 0.133f;
                weaponDamage = 15;
                break;
            default:
                weaponDelay = 0.5f;
                weaponDamage = 10;
                break;
        }
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time - currentTime > weaponDelay)
        {
            currentTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje nabój z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            bullet.GetComponent<Bullet>().bulletDamage = weaponDamage;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
        }
    }
}
