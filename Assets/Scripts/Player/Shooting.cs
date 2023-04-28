using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//KK: Aby to gowno dzialalo, trzeba zmienic ilosc arrayow w inspektorze.
[System.Serializable]
public class AudioClipArray
{
    public AudioClip[] clips;
}

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform firePoint, firePointShotgun1, firePointShotgun2, firePointShotgun3, firePointShotgun4;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    [SerializeField] ShopManager shopManager;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Sprite PlayerPistol, PlayerRifle;
    [SerializeField] AudioClip EmptyMagClip, EmptyEverythingClip, AmmoDropClip;
    private SpriteRenderer spriteRenderer;

    private ScreenShake screenShake;

    bool canShootGlobal = true;
    bool[] canShoot = new bool[5];
    public bool[] isReloading = new bool[5];
    bool[] equippedBefore = new bool[5];
    string[] weaponName = new string[5];
    float bulletForce = 20f;
    float currentTime;
    float[] bulletSpread = new float[5];
    float[] runningBulletSpread = new float[5];
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    int equippedWeaponID;
    public int[] currentAmmo = new int[5];
    public int[] reserveAmmo = new int[5];
    int[] weaponDamage = new int[5];
    int[] maxAmmo = new int[5];

    public AudioClipArray[] weaponAudioClips;
    private IEnumerator weaponCooldownCoroutine;
    private IEnumerator currentReloadCoroutine;

    void Start()
    {
        weaponName[0] = "Pistol";
        weaponDamage[0] = 16;
        weaponDelay[0] = 0.25f;
        reloadTime[0] = 1f;
        maxAmmo[0] = 21;
        bulletSpread[0] = 2f;
        runningBulletSpread[0] = 4f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 32;
        weaponDelay[1] = 0.375f;
        reloadTime[1] = 1f;
        maxAmmo[1] = 9;
        bulletSpread[1] = 0f;
        runningBulletSpread[1] = 2f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 10;
        weaponDelay[2] = 0.08f;
        reloadTime[2] = 2f; 
        maxAmmo[2] = 60;
        bulletSpread[2] = 6f;
        runningBulletSpread[2] = 9f;

        weaponName[3] = "Shotgun";
        weaponDamage[3] = 25;
        weaponDelay[3] = 1f;
        reloadTime[3] = 3f;
        maxAmmo[3] = 7;
        bulletSpread[3] = 4f;
        runningBulletSpread[3] = 8f;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 20;
        weaponDelay[4] = 0.133f;
        reloadTime[4] = 2f;
        maxAmmo[4] = 40;
        bulletSpread[4] = 4f;
        runningBulletSpread[4] = 40f;

        for (int i = 0; i < 5; i++)
        {
            weaponAudioClips[i].clips = Resources.LoadAll<AudioClip>("Audio/Weapons/" + weaponName[i]);
        }

        screenShake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<ScreenShake>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        for(int i = 0; i < 5; i++)
        {
            reserveAmmo[i] = maxAmmo[i] * 2;
            currentAmmo[i] = maxAmmo[i];
            canShoot[i] = true;
        }

        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");

        healthBar.SetWeaponIcon(equippedWeaponID);
        healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        healthBar.SetReloading(false);

        if (equippedWeaponID < 2)
        {
            spriteRenderer.sprite = PlayerPistol;
        }
        else
        {
            spriteRenderer.sprite = PlayerRifle;
        }

        if (isReloading[equippedWeaponID])
        {
            healthBar.SetReloading(true);
        }
    }

    public void AssignWeapon(int weaponID)
    {
        if (weaponID != equippedWeaponID)
        {
            if (weaponCooldownCoroutine != null) StopCoroutine(weaponCooldownCoroutine);
            weaponCooldownCoroutine = WeaponSwitchCooldown();
            StartCoroutine(weaponCooldownCoroutine);
            if (currentReloadCoroutine != null)
            {
                StopCoroutine(currentReloadCoroutine);
                isReloading[equippedWeaponID] = false;
            }
            if (shopManager.shopItems[3, weaponID] == 1)
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
            else
            {
                healthBar.MessageBox("You don't own " + weaponName[weaponID] + "!");
            }
            if (currentAmmo[equippedWeaponID] == 0)
            {
                currentReloadCoroutine = Reload(weaponID);
                StartCoroutine(currentReloadCoroutine);
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentReloadCoroutine = Reload(equippedWeaponID);
            StartCoroutine(currentReloadCoroutine);
        }

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AssignWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AssignWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AssignWeapon(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AssignWeapon(4);
        }
    }

    public void AddAmmo()
    {
        for(int i = 0; i < 5; i++)
        {
            if(shopManager.shopItems[3, i] == 1) reserveAmmo[i] += maxAmmo[i] / 3;
        }
        healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        if (currentAmmo[equippedWeaponID] == 0)
        {
            currentReloadCoroutine = Reload(equippedWeaponID);
            StartCoroutine(currentReloadCoroutine);
        }
    }

    //KK: Rozczytaj sie z tego :DDD
    IEnumerator Shoot()
    {
        if (canShootGlobal && canShoot[equippedWeaponID] && currentAmmo[equippedWeaponID] > 0 && !isReloading[equippedWeaponID] && !PauseMenu.isPaused && !Player.inInventory && !NextLevelScreen.isActive)
        {
            float currentWeaponSpread;
            if (gameObject.GetComponent<Player>().isSprinting || gameObject.GetComponent<Player>().isDashing) currentWeaponSpread = runningBulletSpread[equippedWeaponID];
            else currentWeaponSpread = bulletSpread[equippedWeaponID];
            if (gameObject.GetComponent<Player>().canBetterAim) currentWeaponSpread *= 0.5f;

            canShoot[equippedWeaponID] = false;
            int damage = Random.Range(weaponDamage[equippedWeaponID] - 5, weaponDamage[equippedWeaponID] + 3);
            float randomVal = Random.Range(90f - currentWeaponSpread, 90f + currentWeaponSpread); //KK: Randomowa liczba na spread broni
            Vector3 spread = new Vector3(0, 0, randomVal - 90); 

            screenShake.CamShake(); //KK: Trzesienie kamera
            currentAmmo[equippedWeaponID]--;
            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + spread)); //KK: Spawnuje naboj z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            bullet.GetComponent<Bullet>().bulletDamage = damage;
            bullet.GetComponent<Bullet>().playerIsOwner = true; //KK: Aby kula gracza go nie uderzyla
            bullet.GetComponent<Bullet>().weaponID = equippedWeaponID;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);

            GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);

            if (equippedWeaponID == 3) //shotgun
            {
                GameObject bullet1 = Instantiate(bulletPrefab, firePointShotgun1.position, Quaternion.Euler(firePointShotgun1.rotation.eulerAngles + spread));
                bullet1.GetComponent<Bullet>().weaponID = null;
                bullet1.GetComponent<Bullet>().playerIsOwner = true;
                bullet1.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
                rb1.AddForce(bullet1.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet1, 10);

                GameObject bullet2 = Instantiate(bulletPrefab, firePointShotgun2.position, Quaternion.Euler(firePointShotgun2.rotation.eulerAngles + spread));
                bullet2.GetComponent<Bullet>().weaponID = null;
                bullet2.GetComponent<Bullet>().playerIsOwner = true;
                bullet2.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                rb2.AddForce(bullet2.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet2, 10);

                GameObject bullet3 = Instantiate(bulletPrefab, firePointShotgun3.position, Quaternion.Euler(firePointShotgun3.rotation.eulerAngles + spread));
                bullet3.GetComponent<Bullet>().weaponID = null;
                bullet3.GetComponent<Bullet>().playerIsOwner = true;
                bullet3.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb3 = bullet3.GetComponent<Rigidbody2D>();
                rb3.AddForce(bullet3.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet3, 10);

                GameObject bullet4 = Instantiate(bulletPrefab, firePointShotgun4.position, Quaternion.Euler(firePointShotgun4.rotation.eulerAngles + spread));
                bullet4.GetComponent<Bullet>().weaponID = null;
                bullet4.GetComponent<Bullet>().playerIsOwner = true;
                bullet4.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb4 = bullet4.GetComponent<Rigidbody2D>();
                rb4.AddForce(bullet4.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet4, 10);
            }

            if (currentAmmo[equippedWeaponID] == 0 && reserveAmmo[equippedWeaponID] == 0)
            {
                AudioSource.PlayClipAtPoint(EmptyEverythingClip, transform.position, 1f);
            }
            else if (currentAmmo[equippedWeaponID] == 0)
            {
                currentReloadCoroutine = Reload(equippedWeaponID);
                StartCoroutine(currentReloadCoroutine);
            }
            Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
            Destroy(shootEffect, 1);

            int weaponID = equippedWeaponID; 
            yield return new WaitForSeconds(weaponDelay[equippedWeaponID]);
            canShoot[weaponID] = true;
        }
    }

    IEnumerator WeaponSwitchCooldown()
    {
        canShootGlobal = false;
        yield return new WaitForSeconds(0.25f);
        canShootGlobal = true;
    }

    IEnumerator Reload(int weaponID)
    {
        if (currentAmmo[weaponID] != maxAmmo[weaponID] && reserveAmmo[weaponID] > 0 && isReloading[weaponID] == false)
        {
            isReloading[weaponID] = true;
            AudioSource.PlayClipAtPoint(EmptyMagClip, transform.position, 1f);
            float time = reloadTime[weaponID];
            while(time > 0)
            {
                if (equippedWeaponID == weaponID)
                {
                    healthBar.SetReloading(true, time);
                    time -= 0.1f;
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    isReloading[weaponID] = false;
                    healthBar.SetReloading(false);
                    yield break;
                }
            }

            //KK: Jestem pewny, ze da sie tego ifa zrobic o wiele lepiej. Nie chce mi sie :)
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
            if (equippedWeaponID == weaponID)
            {
                healthBar.SetReloading(false);
                AudioSource.PlayClipAtPoint(AmmoDropClip, transform.position, 1f);
            }

            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        }
    }
}
