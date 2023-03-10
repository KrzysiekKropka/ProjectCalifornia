using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject shootPrefab;
    public HealthBar healthBar;

    bool isReloading = false;
    float weaponDelay;
    float reloadTime;
    float bulletForce = 30f;
    float currentTime;
    int equippedWeaponID;
    int weaponDamage; 
    int currentAmmo; 
    int maxAmmo; 
    int reserveAmmo;

    void Start()
    {
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        switch (equippedWeaponID)
        {
            case 1:
                weaponDelay = 0.75f;
                weaponDamage = 35;
                maxAmmo = 7;
                reloadTime = 2f;
                break;
            case 2:
                weaponDelay = 0.1f;
                weaponDamage = 10;
                maxAmmo = 50;
                reloadTime = 3f;
                break;
            case 3:
                weaponDelay = 1f;
                weaponDamage = 35;
                maxAmmo = 5;
                reloadTime = 5f;
                break;
            case 4:
                weaponDelay = 0.133f;
                weaponDamage = 20; 
                maxAmmo = 30;
                reloadTime = 3f;
                break;
            default:
                weaponDelay = 0.5f;
                weaponDamage = 10;
                maxAmmo = 17;
                reloadTime = 2f;
                break;
        }
        reserveAmmo = maxAmmo * 4;
        currentAmmo = maxAmmo;
        healthBar.SetAmmo(currentAmmo, reserveAmmo);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && isReloading == false)
        {
            Shoot();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (Time.time - currentTime > weaponDelay && currentAmmo > 0)
        {
            currentAmmo--;
            healthBar.SetAmmo(currentAmmo, reserveAmmo);
            currentTime = Time.time;
            GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje naboj z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            bullet.GetComponent<PlayerBullet>().bulletDamage = weaponDamage;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
            Destroy(shootEffect, 1);
        }
    }

    IEnumerator Reload()
    {
        if (currentAmmo != maxAmmo && reserveAmmo > 0 && isReloading == false)
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);
            if ((currentAmmo + reserveAmmo) > maxAmmo)
            {
                int reloadAmount = maxAmmo - currentAmmo;
                currentAmmo += reloadAmount;
                reserveAmmo -= reloadAmount;
            }
            else
            {
                currentAmmo += reserveAmmo;
                reserveAmmo = 0;
            }
            isReloading = false;
            healthBar.SetAmmo(currentAmmo, reserveAmmo);
        }
    }
}
