using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shootPrefab;
    [SerializeField] Sprite EnemyPistol, EnemyDeagle, EnemyMP5, EnemyAK47;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    bool isReloading;
    bool readyToShoot;
    bool canShoot = true;
    string[] weaponName = new string[5];
    float currentTime;
    float reactionTime = 0.25f;
    float[] bulletSpread = new float[5];
    float[] weaponDelay = new float[5];
    float[] reloadTime = new float[5];
    public int equippedWeaponID;
    public int[] currentAmmo = new int[5];
    int[] weaponDamage = new int[5];
    int[] maxAmmo = new int[5];
    float bulletForce = 25f;
    float reach = 20f;
    float timer;

    void Start()
    {
        int isStatic = gameObject.GetComponent<AIBrain>().isStatic ? 0 : 1;

        weaponName[0] = "Pistol";
        weaponDamage[0] = 5;
        weaponDelay[0] = 0.5f;
        reloadTime[0] = 3f;
        maxAmmo[0] = 17;
        bulletSpread[0] = 4f;

        weaponName[1] = "Deagle";
        weaponDamage[1] = 20;
        weaponDelay[1] = 0.75f;
        reloadTime[1] = 3f;
        maxAmmo[1] = 7;
        bulletSpread[1] = 3f;

        weaponName[2] = "MP5";
        weaponDamage[2] = 5;
        weaponDelay[2] = 0.2f;
        reloadTime[2] = 5f;
        maxAmmo[2] = 50;
        bulletSpread[2] = 9f;

        /*weaponName[3] = "Shotgun";
        weaponDamage[3] = 10;
        weaponDelay[3] = 0.8f;
        reloadTime[3] = 6f;
        maxAmmo[3] = 6;
        bulletSpread[3] = 3f;*/

        weaponName[4] = "AK-47";
        weaponDamage[4] = 15;
        weaponDelay[4] = 0.4f;
        reloadTime[4] = 5f;
        maxAmmo[4] = 30;
        bulletSpread[4] = 6f;

        for(int i = 0; i < 5; i++)
        {
            bulletSpread[i] = bulletSpread[i] * Mathf.Pow(1.5f, isStatic);
        }

        currentAmmo[equippedWeaponID] = maxAmmo[equippedWeaponID];

        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        //KK: Fuj
        switch (equippedWeaponID)
        {
            case 0:
                spriteRenderer.sprite = EnemyPistol;
                gameObject.GetComponent<AIBrain>().dropHP = 5;
                gameObject.GetComponent<AIBrain>().dropMoney = 100;
                gameObject.GetComponent<AIBrain>().maxHealth = 50;
                break;
            case 1:
                spriteRenderer.sprite = EnemyDeagle;
                gameObject.GetComponent<AIBrain>().dropHP = 10;
                gameObject.GetComponent<AIBrain>().dropMoney = 200;
                gameObject.GetComponent<AIBrain>().maxHealth = 75;
                break;
            case 2:
                spriteRenderer.sprite = EnemyMP5;
                gameObject.GetComponent<AIBrain>().dropHP = 15;
                gameObject.GetComponent<AIBrain>().dropMoney = 300;
                gameObject.GetComponent<AIBrain>().maxHealth = 100;
                break;
            case 3:
                break;
            case 4:
                spriteRenderer.sprite = EnemyAK47;
                gameObject.GetComponent<AIBrain>().dropHP = 20;
                gameObject.GetComponent<AIBrain>().dropMoney = 400;
                gameObject.GetComponent<AIBrain>().maxHealth = 150;
                break;
        }

        healthBar.SetAmmo(currentAmmo[equippedWeaponID]);
    }

    void Update()
    {
        RayCasting();
    }

    void RayCasting()
    {
        RaycastHit2D detectRay = Physics2D.Raycast(transform.position, transform.up);


        //KK: Bardziej debilnego kodu napisac nie moglem
        if (detectRay.collider != null)
        {
            if (detectRay.collider.tag == "Player" && detectRay.distance < reach)
            {
                GetComponent<AIBrain>().PlayerInterrupts();
                readyToShoot = true;
                if (timer < reactionTime*2) timer += Time.deltaTime;
                else timer = reactionTime*2;
            }
            else
            {
                readyToShoot = false;
                if(timer>0)timer -= Time.deltaTime;
                else timer = 0;
            }
        }

        if (readyToShoot) StartCoroutine(Shoot());
        Debug.DrawRay(transform.position, transform.up * reach, Color.red);
    }

    IEnumerator Shoot()
    {
        if (timer >= reactionTime && canShoot && !isReloading && readyToShoot)
        {
            canShoot = false;
            float randomVal = Random.Range(90f - bulletSpread[equippedWeaponID], 90f + bulletSpread[equippedWeaponID]); //KK: Randomowa liczba na spread broni
            Vector3 spread = new Vector3(0, 0, randomVal - 90);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(firePoint.rotation.eulerAngles + spread)); //KK: Spawnuje nab�j z pozycja objektu firePoint znajdujacego sie na obiekcie gracza na koncu broni
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            currentAmmo[equippedWeaponID]--;
            healthBar.SetAmmo(currentAmmo[equippedWeaponID]);

            bullet.GetComponent<Bullet>().bulletDamage = weaponDamage[equippedWeaponID];
            bullet.GetComponent<Bullet>().enemyIsOwner = true;
            bullet.GetComponent<Bullet>().weaponID = equippedWeaponID;

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
