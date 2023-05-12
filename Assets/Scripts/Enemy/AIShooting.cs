using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : MonoBehaviour
{
    //Objects
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private Sprite EnemyPistol, EnemyDeagle, EnemyMP5, EnemyAK47;
    [SerializeField] private GameObject enemy;
    private GameObject player;
    private AIBrain brain;
    private SpriteRenderer spriteRenderer;

    //Boss stuff
    [SerializeField] private bool customStats;
    [SerializeField] private int customDamage;
    [SerializeField] private int customAmmo;
    [SerializeField] private float customDelay;
    [SerializeField] private float customSpread;
    [SerializeField] private Sprite customSprite;

    //Weapon related
    private bool isReloading;
    private bool canShoot = true;
    public int equippedWeaponID;
    private float bulletForce = 20f;
    private float reach;
    private string[] weaponName = new string[5];
    private float[] bulletSpread = new float[5];
    private float[] weaponDelay = new float[5];
    private float[] reloadTime = new float[5];
    public int[] currentAmmo = new int[5];
    private int[] weaponDamage = new int[5];
    private int[] maxAmmo = new int[5];

    //Timer
    private float timer;

    void Start()
    {
        reach = enemy.GetComponent<FieldOfView>().radius*1.5f;

        weaponName[0] = "Pistol";
        weaponDamage[0] = 6;
        weaponDelay[0] = 0.5f;
        reloadTime[0] = 2f;
        maxAmmo[0] = 21;
        bulletSpread[0] = 3f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 12;
        weaponDelay[1] = 0.75f;
        reloadTime[1] = 2f;
        maxAmmo[1] = 7;
        bulletSpread[1] = 1f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 8;
        weaponDelay[2] = 0.16f;
        reloadTime[2] = 4f;
        maxAmmo[2] = 60;
        bulletSpread[2] = 5f;

        weaponName[4] = "AK-47";
        weaponDamage[4] = 16;
        weaponDelay[4] = 0.375f;
        reloadTime[4] = 4f;
        maxAmmo[4] = 40;
        bulletSpread[4] = 3f;

        currentAmmo[equippedWeaponID] = maxAmmo[equippedWeaponID];

        player = GameObject.FindGameObjectWithTag("Player");
        brain = enemy.GetComponent<AIBrain>();
        spriteRenderer = enemy.GetComponent<SpriteRenderer>();

        //KK: Fuj
        if(!brain.isDreamy)
        {
            switch (equippedWeaponID)
            {
                case 0:
                    spriteRenderer.sprite = EnemyPistol;
                    brain.dropHP = 5;
                    brain.dropXP = 30;
                    brain.dropMoney = 150;
                    if (!brain.customHP) brain.maxHealth = 50;
                    break;
                case 1:
                    spriteRenderer.sprite = EnemyDeagle;
                    brain.dropHP = 10;
                    brain.dropXP = 45;
                    brain.dropMoney = 300;
                    if (!brain.customHP) brain.maxHealth = 75;
                    break;
                case 2:
                    spriteRenderer.sprite = EnemyMP5;
                    brain.dropHP = 15;
                    brain.dropXP = 60;
                    brain.dropMoney = 450;
                    if (!brain.customHP) brain.maxHealth = 100;
                    break;
                case 3:
                    break;
                case 4:
                    spriteRenderer.sprite = EnemyAK47;
                    brain.dropHP = 30;
                    brain.dropXP = 90;
                    brain.dropMoney = 600;
                    if (!brain.customHP) brain.maxHealth = 150;
                    break;
            }
        }

        if(customStats)
        {
            weaponDamage[equippedWeaponID] = customDamage;
            weaponDelay[equippedWeaponID] = customDelay;
            bulletSpread[equippedWeaponID] = customSpread;
            currentAmmo[equippedWeaponID] = customAmmo;
            maxAmmo[equippedWeaponID] = customAmmo;
            if (customSprite!=null)spriteRenderer.sprite = customSprite;
        }

        healthBar.SetAmmo(currentAmmo[equippedWeaponID]);
    }

    void Update()
    {
        RayCasting();
    }

    void RayCasting()
    {
        if (brain.playerDetected)
        {
            RaycastHit2D detectRay = Physics2D.Raycast(enemy.transform.position, enemy.transform.up);

            //KK: Bardziej debilnego kodu napisac nie moglem
            if (detectRay.collider != null)
            {
                if (detectRay.collider.tag == "Player" && detectRay.distance < reach)
                {
                    if(brain.rotationSpeed != brain.lockedRotationSpeed)brain.ChangeAimLock(true);
                    StartCoroutine(Shoot());
                }
                else
                {
                    if (timer > 0) timer -= Time.deltaTime;
                    else timer = 0;
                }
            }
        }
        //Debug.DrawRay(enemy.transform.position, enemy.transform.up * reach, Color.red);
    }

    IEnumerator Shoot()
    {
        //if (timer >= reactionTime && canShoot && !isReloading && readyToShoot)
        if (canShoot && !isReloading)
        {
            float currentSpread = bulletSpread[equippedWeaponID];
            if (brain.isStatic) currentSpread /= 2;
            canShoot = false;
            float randomVal = Random.Range(90f - currentSpread, 90f + currentSpread); //KK: Randomowa liczba na spread broni
            Vector3 spread = new Vector3(0, 0, randomVal - 90);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + spread)); //KK: Spawnuje nabój z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            currentAmmo[equippedWeaponID]--;
            healthBar.SetAmmo(currentAmmo[equippedWeaponID]);

            bulletScript.bulletDamage = weaponDamage[equippedWeaponID];
            bulletScript.enemyIsOwner = true;
            bulletScript.weaponID = equippedWeaponID;

            GameObject shootEffect = Instantiate(shootPrefab, firePoint.position, firePoint.rotation);

            Destroy(bullet, 10);
            Destroy(shootEffect, 1);

            if (currentAmmo[equippedWeaponID] == 0)
            {
                StartCoroutine(Reload(equippedWeaponID));
            }

            yield return new WaitForSeconds(weaponDelay[equippedWeaponID]);
            canShoot = true;
        }
    }

    IEnumerator Reload(int weaponID)
    {
        if (currentAmmo[weaponID] != maxAmmo[weaponID] && isReloading == false)
        {
            isReloading = true;
            healthBar.SetReloading(true);

            yield return new WaitForSeconds(reloadTime[weaponID]);
            currentAmmo[weaponID] = maxAmmo[weaponID];
            isReloading = false;
            healthBar.SetReloading(false);
            healthBar.SetAmmo(currentAmmo[equippedWeaponID]);
        }
    }
}
