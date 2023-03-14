using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform firePoint, firePointShotgun1, firePointShotgun2;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Sprite PlayerPistol, PlayerRifle;
    private SpriteRenderer spriteRenderer;

    public ScreenShake screenShake;

    public bool[] isReloading = new bool[5];
    bool[] equippedBefore = new bool[5];
    string[] weaponName = new string[5];
    float bulletForce = 30f;
    float currentTime;
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    public int equippedWeaponID;
    int[] weaponDamage = new int[5];
    public int[] currentAmmo = new int[5];
    public int[] reserveAmmo = new int[5];
    int[] maxAmmo = new int[5];

    void Start()
    {
        weaponName[0] = "Pistol";
        weaponDamage[0] = 10;
        weaponDelay[0] = 0.25f;
        reloadTime[0] = 2f;
        maxAmmo[0] = 17;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 30;
        weaponDelay[1] = 0.5f;
        reloadTime[1] = 2f;
        maxAmmo[1] = 7;

        weaponName[2] = "MP5";
        weaponDamage[2] = 10;
        weaponDelay[2] = 0.1f;
        reloadTime[2] = 3f; 
        maxAmmo[2] = 50;

        weaponName[3] = "Shotgun";
        weaponDamage[3] = 20;
        weaponDelay[3] = 0.8f;
        reloadTime[3] = 4f;
        maxAmmo[3] = 5;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 20;
        weaponDelay[4] = 0.133f;
        reloadTime[4] = 3f;
        maxAmmo[4] = 30;

        screenShake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<ScreenShake>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        for(int i = 0; i < 5; i++)
        {
            reserveAmmo[i] = maxAmmo[i] * 3;
            currentAmmo[i] = maxAmmo[i];
        }

        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        AssignWeapon(equippedWeaponID);
    }

    public void AssignWeapon(int weaponID)
    {
        healthBar.SetWeaponIcon(weaponID);
        healthBar.SetAmmo(currentAmmo[weaponID], reserveAmmo[weaponID]);
        equippedWeaponID = weaponID;
        healthBar.SetReloading(false);

        if (weaponID < 2)
        {
            spriteRenderer.sprite = PlayerPistol;
        }
        else
        {
            spriteRenderer.sprite = PlayerRifle;
        }

        if (isReloading[weaponID])
        {
            healthBar.SetReloading(true);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload(equippedWeaponID));
        }
    }

    void Shoot()
    {
        if (Time.time - currentTime > weaponDelay[equippedWeaponID] && currentAmmo[equippedWeaponID] > 0 && Player.inInventory == false && isReloading[equippedWeaponID] == false)
        {
            screenShake.CamShake();
            currentTime = Time.time;
            currentAmmo[equippedWeaponID]--;
            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //KK: Spawnuje naboj z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            bullet.GetComponent<Bullet>().bulletDamage = weaponDamage[equippedWeaponID];
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
            Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
            Destroy(shootEffect, 1);
            if (equippedWeaponID == 3) //shotgun
            {
                GameObject bullet1 = Instantiate(bulletPrefab, firePointShotgun1.position, firePointShotgun1.rotation);
                bullet1.GetComponent<Bullet>().bulletDamage = weaponDamage[equippedWeaponID];
                Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
                rb1.AddForce(firePointShotgun1.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet1, 10);

                GameObject bullet2 = Instantiate(bulletPrefab, firePointShotgun2.position, firePointShotgun2.rotation);
                bullet2.GetComponent<Bullet>().bulletDamage = weaponDamage[equippedWeaponID];
                Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                rb2.AddForce(firePointShotgun2.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet2, 10);
            }
        }
    }

    IEnumerator Reload(int weaponID)
    {
        if (currentAmmo[weaponID] != maxAmmo[weaponID] && reserveAmmo[weaponID] > 0 && isReloading[weaponID] == false)
        {
            isReloading[weaponID] = true;
            healthBar.SetReloading(true);
            yield return new WaitForSeconds(reloadTime[weaponID]);
            if ((currentAmmo[weaponID] + reserveAmmo[weaponID]) > maxAmmo[weaponID])
            {
                int reloadAmount = maxAmmo[weaponID] - currentAmmo[weaponID];
                currentAmmo[weaponID] += reloadAmount;
                reserveAmmo[weaponID] -= reloadAmount;
            }
            else
            {
                currentAmmo[weaponID] += reserveAmmo[weaponID];
                reserveAmmo[weaponID] = 0;
            }
            isReloading[weaponID] = false;
            if(equippedWeaponID == weaponID)healthBar.SetReloading(false);
            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        }
    }
}
