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
    //Objects
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform[] firePointShotgun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private InventoryButtonInfo[] inventoryButtonInfos;
    [SerializeField] private Sprite PlayerPistol, PlayerRifle;
    [SerializeField] private AudioClip EmptyMagClip, EmptyEverythingClip, AmmoDropClip;
    private SpriteRenderer spriteRenderer;
    private ScreenShake screenShake;

    //Global Bools
    private bool canShootGlobal = true;

    //Weapon Stats
    private float bulletForce = 20f;
    private int equippedWeaponID;
    private bool[] canShoot = new bool[5];
    public bool[] isReloading = new bool[5];
    private bool[] equippedBefore = new bool[5];
    private string[] weaponName = new string[5];
    private float[] bulletSpread = new float[5];
    private float[] runningBulletSpread = new float[5];
    private float[] weaponDelay = new float[5];
    private float[] reloadTime = new float[5];
    public int[] currentAmmo = new int[5];
    public int[] reserveAmmo = new int[5];
    private int[] weaponDamage = new int[5];
    private int[] maxAmmo = new int[5];

    //Array of weapon audio clips
    public AudioClipArray[] weaponAudioClips;

    //IEnumerators for storing coroutines
    private IEnumerator weaponCooldownCoroutine, currentReloadCoroutine;

    void Start()
    {
        weaponName[0] = "Pistol";
        weaponDamage[0] = 16;
        weaponDelay[0] = 0.25f;
        reloadTime[0] = 0.5f;
        maxAmmo[0] = 21;
        bulletSpread[0] = 2f;
        runningBulletSpread[0] = 4f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 32;
        weaponDelay[1] = 0.375f;
        reloadTime[1] = 0.5f;
        maxAmmo[1] = 9;
        bulletSpread[1] = 0f;
        runningBulletSpread[1] = 2f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 12;
        weaponDelay[2] = 0.08f;
        reloadTime[2] = 1f; 
        maxAmmo[2] = 60;
        bulletSpread[2] = 6f;
        runningBulletSpread[2] = 6f;

        weaponName[3] = "Shotgun";
        weaponDamage[3] = 25;
        weaponDelay[3] = 1f;
        reloadTime[3] = 1f;
        maxAmmo[3] = 7;
        bulletSpread[3] = 2f;
        runningBulletSpread[3] = 4f;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 25;
        weaponDelay[4] = 0.125f;
        reloadTime[4] = 1.5f;
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

        healthBar.SetReloading(false);
        healthBar.SetWeaponIcon(equippedWeaponID);
        healthBar.SetAmmo(currentAmmo[equippedWeaponID], reserveAmmo[equippedWeaponID]);

        if (equippedWeaponID < 2)
        {
            spriteRenderer.sprite = PlayerPistol;
        }
        else
        {
            spriteRenderer.sprite = PlayerRifle;
        }
    }

    public void AssignWeapon(int weaponID)
    {
        if (weaponID != equippedWeaponID && shopManager.shopItems[3, weaponID] == 1)
        {
            if (weaponCooldownCoroutine != null) StopCoroutine(weaponCooldownCoroutine);
            weaponCooldownCoroutine = WeaponSwitchCooldown();
            StartCoroutine(weaponCooldownCoroutine);

            if (currentReloadCoroutine != null)
            {
                StopCoroutine(currentReloadCoroutine);
                isReloading[equippedWeaponID] = false;
            }

            healthBar.SetReloading(false);
            healthBar.SetWeaponIcon(weaponID);
            healthBar.SetAmmo(currentAmmo[weaponID], reserveAmmo[weaponID]);

            if (weaponID < 2)
            {
                spriteRenderer.sprite = PlayerPistol;
            }
            else
            {
                spriteRenderer.sprite = PlayerRifle;
            }

            PlayerPrefs.SetInt("equippedWeaponID", weaponID);

            equippedWeaponID = weaponID;

            if (currentAmmo[equippedWeaponID] == 0)
            {
                currentReloadCoroutine = Reload(equippedWeaponID);
                StartCoroutine(currentReloadCoroutine);
            }
        }
        else if (shopManager.shopItems[3, weaponID] != 1)
        {
            healthBar.MessageBox("You don't own " + weaponName[weaponID] + "!");
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

            if (equippedWeaponID == 3) // Shotgun
            {
                GameObject[] bullets = new GameObject[firePointShotgun.Length];
                Rigidbody2D[] rbs = new Rigidbody2D[firePointShotgun.Length];

                for (int i=0; i< firePointShotgun.Length; i++)
                {
                    bullets[i] = Instantiate(bulletPrefab, firePointShotgun[i].position, Quaternion.Euler(firePointShotgun[i].rotation.eulerAngles + spread));
                    bullets[i].GetComponent<Bullet>().weaponID = null;
                    bullets[i].GetComponent<Bullet>().playerIsOwner = true;
                    bullets[i].GetComponent<Bullet>().bulletDamage = damage;
                    rbs[i] = bullets[i].GetComponent<Rigidbody2D>();
                    rbs[i].AddForce(bullets[i].transform.up * bulletForce, ForceMode2D.Impulse);
                    Destroy(bullets[i], 10);
                }
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
        if (currentAmmo[weaponID] < maxAmmo[weaponID] && reserveAmmo[weaponID] > 0 && isReloading[weaponID] == false)
        {
            isReloading[weaponID] = true;
            AudioSource.PlayClipAtPoint(EmptyMagClip, transform.position, 1f);
            float time = reloadTime[weaponID];
            healthBar.reloadSlider.maxValue = time;
            while(time > 0)
            {
                if(equippedWeaponID == weaponID)
                {
                    healthBar.SetReloading(true, time);
                    time -= 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    healthBar.SetReloading(false);
                    isReloading[weaponID] = false;
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
            healthBar.SetReloading(false);
            if(inventoryButtonInfos[weaponID] != null) inventoryButtonInfos[weaponID].AssignOwnership();
            AudioSource.PlayClipAtPoint(AmmoDropClip, transform.position, 1f);
            healthBar.SetAmmo(currentAmmo[weaponID], reserveAmmo[weaponID]);
        }
    }
}
