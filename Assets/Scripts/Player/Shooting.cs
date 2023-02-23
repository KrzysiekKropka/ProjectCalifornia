using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public int equippedWeaponID;
    public int weaponDamage;
    public float weaponDelay;
    public float bulletForce = 10f;
    bool canShoot = true;

    void Start()
    {
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");

        switch (equippedWeaponID)
        {
            case 1:
                weaponDelay = 0.5f;
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
                weaponDelay = 0.125f;
                weaponDamage = 15;
                break;
            default:
                weaponDelay = 0.35f;
                weaponDamage = 10;
                break;
        }
    }


    void Update()
    {
        if(!PauseMenu.isPaused && Input.GetMouseButton(0))
        {
            if(canShoot)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje nabój z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
        bullet.GetComponent<Bullet>().bulletDamage = weaponDamage;
        Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(weaponDelay);
        canShoot = true;
    }
}
