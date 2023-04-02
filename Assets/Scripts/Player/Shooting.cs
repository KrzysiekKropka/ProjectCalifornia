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
    [SerializeField] Transform firePoint, firePointShotgun1, firePointShotgun2;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Sprite PlayerPistol, PlayerRifle;
    [SerializeField] AudioClip EmptyMagClip, EmptyEverythingClip, AmmoDropClip;
    private SpriteRenderer spriteRenderer;

    public ScreenShake screenShake;

    public bool[] isReloading = new bool[5];
    bool[] equippedBefore = new bool[5];
    string[] weaponName = new string[5];
    float bulletForce = 25f;
    float currentTime;
    float[] bulletSpread = new float[5];
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    public int equippedWeaponID;
    public int[] currentAmmo = new int[5];
    public int[] reserveAmmo = new int[5];
    int[] weaponDamage = new int[5];
    int[] maxAmmo = new int[5];

    public AudioClipArray[] weaponAudioClips;

    void Start()
    {
        weaponName[0] = "Pistol";
        weaponDamage[0] = 10;
        weaponDelay[0] = 0.35f;
        reloadTime[0] = 3f;
        maxAmmo[0] = 17;
        bulletSpread[0] = 1.5f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 30;
        weaponDelay[1] = 0.5f;
        reloadTime[1] = 3f;
        maxAmmo[1] = 7;
        bulletSpread[1] = 0f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 10;
        weaponDelay[2] = 0.09f;
        reloadTime[2] = 5f; 
        maxAmmo[2] = 50;
        bulletSpread[2] = 5f;

        weaponName[3] = "Shotgun";
        weaponDamage[3] = 20;
        weaponDelay[3] = 1f;
        reloadTime[3] = 5f;
        maxAmmo[3] = 6;
        bulletSpread[3] = 3f;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 20;
        weaponDelay[4] = 0.18f;
        reloadTime[4] = 5f;
        maxAmmo[4] = 30;
        bulletSpread[4] = 2.5f;

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
            int damage = Random.Range(weaponDamage[equippedWeaponID] - 5, weaponDamage[equippedWeaponID] + 3);
            float randomVal = Random.Range(90f - bulletSpread[equippedWeaponID], 90f + bulletSpread[equippedWeaponID]);
            Vector3 spread = new Vector3(0, 0, randomVal - 90);
            screenShake.CamShake();
            currentTime = Time.time;
            currentAmmo[equippedWeaponID]--;
            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + spread)); //KK: Spawnuje naboj z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            bullet.GetComponent<Bullet>().bulletDamage = damage;
            bullet.GetComponent<Bullet>().playerIsOwner = true;
            bullet.GetComponent<Bullet>().weaponID = equippedWeaponID;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);
            if (equippedWeaponID == 3) //shotgun
            {
                GameObject bullet1 = Instantiate(bulletPrefab, firePointShotgun1.position, Quaternion.Euler(firePointShotgun1.rotation.eulerAngles + spread));
                bullet1.GetComponent<Bullet>().weaponID = -1;
                bullet1.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
                rb1.AddForce(bullet1.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet1, 10);

                GameObject bullet2 = Instantiate(bulletPrefab, firePointShotgun2.position, Quaternion.Euler(firePointShotgun2.rotation.eulerAngles + spread));
                bullet2.GetComponent<Bullet>().weaponID = -1;
                bullet2.GetComponent<Bullet>().bulletDamage = damage;
                Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
                rb2.AddForce(bullet2.transform.up * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet2, 10);
            }


            if (currentAmmo[equippedWeaponID] == 0 && reserveAmmo[equippedWeaponID] == 0)
            {
                AudioSource.PlayClipAtPoint(EmptyEverythingClip, transform.position, 1f);
            }
            else if (currentAmmo[equippedWeaponID] == 0)
            {
                StartCoroutine(Reload(equippedWeaponID));
                AudioSource.PlayClipAtPoint(EmptyMagClip, transform.position, 1f);
            }
            Destroy(bullet, 10); //KK: Usuwa obiekt po 10 sekundach jesli nie zostanie usuniety przez cos innego
            Destroy(shootEffect, 1);
        }
    }

    public void AddAmmo()
    {
        reserveAmmo[equippedWeaponID] += maxAmmo[equippedWeaponID]/2;
        healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        if (currentAmmo[equippedWeaponID] == 0) StartCoroutine(Reload(equippedWeaponID));
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
            if (equippedWeaponID == weaponID)
            {
                healthBar.SetReloading(false);
                AudioSource.PlayClipAtPoint(AmmoDropClip, transform.position, 1f);
            }

            healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);
        }
    }
}
