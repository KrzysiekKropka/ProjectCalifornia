using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    string[] weaponName = new string[5];
    float currentTime;
    float[] bulletSpread = new float[5];
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    public int equippedWeaponID;
    public int[] currentAmmo = new int[5];
    public int[] reserveAmmo = new int[5];
    int[] weaponDamage = new int[5];
    int[] maxAmmo = new int[5];
    float bulletForce = 25f;
    float distance;
    float timer;

    void Start()
    {
        weaponName[0] = "Pistol";
        weaponDamage[0] = 5;
        weaponDelay[0] = 0.5f;
        reloadTime[0] = 3f;
        maxAmmo[0] = 17;
        bulletSpread[0] = 1.5f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 20;
        weaponDelay[1] = 0.75f;
        reloadTime[1] = 3f;
        maxAmmo[1] = 7;
        bulletSpread[1] = 0f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 5;
        weaponDelay[2] = 0.175f;
        reloadTime[2] = 5f;
        maxAmmo[2] = 50;
        bulletSpread[2] = 5f;

        weaponName[3] = "Shotgun";
        weaponDamage[3] = 10;
        weaponDelay[3] = 0.8f;
        reloadTime[3] = 6f;
        maxAmmo[3] = 6;
        bulletSpread[3] = 3f;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 15;
        weaponDelay[4] = 0.35f;
        reloadTime[4] = 5f;
        maxAmmo[4] = 30;
        bulletSpread[4] = 2.5f;

        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            reserveAmmo[i] = maxAmmo[i] * 3;
            currentAmmo[i] = maxAmmo[i];
        }

        /*if (weaponID < 2)
        {
            spriteRenderer.sprite = PlayerPistol;
        }
        else
        {
            spriteRenderer.sprite = PlayerRifle;
        }*/
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        timer += Time.deltaTime;
        if (timer > weaponDelay[equippedWeaponID] && distance < 15f)
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje nabój z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
        GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().bulletDamage = weaponDamage[equippedWeaponID]; 
        bullet.GetComponent<Bullet>().enemyIsOwner = true;
        bullet.GetComponent<Bullet>().weaponID = equippedWeaponID;
        Destroy(bullet, 10);
        Destroy(shootEffect, 1);
    }
}
