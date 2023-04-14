using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class AIBrain : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject bloodPoolEffect;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] GameObject moneyDropPrefab;
    [SerializeField] GameObject ammoDropPrefab;
    [SerializeField] GameObject medkitPrefab;
    [SerializeField] DialogueBox dialogue;
    [SerializeField] Rigidbody2D rb;

    private string[] SeekingPlayerDialogue = { "Where is he?", "I lost him!" };

    private GameObject player;
    private GameObject damagePopup;
    private AIPath ai;

    float speed = 6f; 
    public int maxHealth = 100;

    public bool isStatic = false;
    public bool playerDetected = false;
    bool playerWasDetected = false;
    bool shotBefore = false;

    int currentHealth;

    float forgetPlayer = 5;
    float forgetPlayerTimer;
    float notifyOthersCooldown = 3;

    public int dropXP;
    public int dropMoney;
    public int dropHP;
    int summedDamage;
    float currentTime, currentTimeBloodPool;
    float rotationSpeed = 0.25f;
    float aimAngle;

    RaycastHit hit;

    Vector3 aimDirection;
    Vector3 playerLocation;

    private IEnumerator stopFollowCoroutine;
    private IEnumerator seekPlayerCoroutine;
    bool seekingActivated;

    Collider2D[] Colliders;

    void Start()
    {
        deadBody.SetActive(false);
        ai = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().AddEnemy();
        ai.maxSpeed = speed;
        aimAngle = rb.rotation;
    }

    void Update()
    {
        //print(forgetPlayerTimer);

        if(forgetPlayerTimer>0)forgetPlayerTimer -= Time.deltaTime;
        Colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
        if (playerDetected && forgetPlayerTimer>notifyOthersCooldown && Colliders.Length > 0)
        {
            foreach (Collider2D Enemy in Colliders)
            {
                if (Enemy.gameObject.tag == "Enemy")
                {
                    if (!Enemy.gameObject.GetComponent<AIBrain>().playerDetected) Enemy.gameObject.GetComponent<AIBrain>().PlayerInterrupts();
                }
            }
        }

        if (playerDetected) playerLocation = player.transform.position;
        if (playerLocation!=null) aimDirection = playerLocation - transform.position;
        if (playerWasDetected) aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        if (!isStatic && playerDetected) ai.destination = player.transform.position;
    }

    void FixedUpdate()
    {
        if(!seekingActivated)
            rb.rotation = Mathf.LerpAngle(rb.rotation, aimAngle, rotationSpeed);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("PlayerBullet"))
        {
            PlayerInterrupts();
        }
    }

    /*void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerBullet"))
        {
            PlayerOutRange();
        }
    }

    public void PlayerInRange()
    {
        if (stopFollowCoroutine != null) StopCoroutine(stopFollowCoroutine);
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekingActivated = false;
        playerDetected = true;
        playerWasDetected = true;
    }

    public void PlayerOutRange()
    {
        if (stopFollowCoroutine != null) StopCoroutine(stopFollowCoroutine);
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekingActivated = false;
        stopFollowCoroutine = StopFollowing();
        StartCoroutine(stopFollowCoroutine);
    }*/

    public void PlayerInterrupts()
    {
        if (stopFollowCoroutine != null) StopCoroutine(stopFollowCoroutine);
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        forgetPlayerTimer = forgetPlayer;
        seekingActivated = false;
        playerDetected = true;
        playerWasDetected = true;
        stopFollowCoroutine = StopFollowing();
        StartCoroutine(stopFollowCoroutine);
    }

    public void TakeDamage(int damage)
    {
        if (!shotBefore)
        {
            currentHealth = maxHealth;
            healthBar.GetComponent<EnemyHealthBar>().SetMaxHealth(maxHealth);
            shotBefore = true;
        }
        currentHealth -= damage;
        //KK:Blagam nie dotykaj tego kodu ponizej, jest on tak kurwa niestabilny ze lekka zmiana kompletnie rozpierdoli dzialanie licznika. To jest niesamowite ze to w ogole dziala.
        if (Time.time - currentTime > 0.75f)
        {
            summedDamage = 0;
        }

        summedDamage += damage;

        if (Time.time - currentTime > 0.1f)
        {
            damagePopup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
            damagePopup.GetComponent<DamagePopup>().SetDamageText(summedDamage);
        }
        else
        {
            damagePopup.GetComponent<DamagePopup>().SetDamageText(summedDamage);
        }
        if (currentHealth <= 0)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            deadBody.SetActive(true);
            deadBody.GetComponent<DeadBodyFollow>().DeadBodyPosition(gameObject.GetComponent<AIShooting>().equippedWeaponID);
            Destroy(gameObject);
            Destroy(healthBar);
            if (Time.time - currentTimeBloodPool > .01f)
            {
                if (player)
                {
                    player.SetXP(dropXP);
                    player.AddKill();
                }
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
                GameObject droppedMoney = Instantiate(moneyDropPrefab, transform.position + new Vector3(-0.75f, -0.75f, 0f), Quaternion.identity);
                droppedMoney.GetComponent<MoneyDrop>().money = dropMoney;
                GameObject droppedAmmo = Instantiate(ammoDropPrefab, transform.position + new Vector3(0.75f, -0.75f, 0f), Quaternion.identity);
                GameObject droppedMedkit = Instantiate(medkitPrefab, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
                droppedMedkit.GetComponent<Medkit>().hp = dropHP;
                currentTimeBloodPool = Time.time;
            }
        }
        healthBar.GetComponent<EnemyHealthBar>().SetHealth(currentHealth);
        currentTime = Time.time;
    }

    IEnumerator StopFollowing()
    {
        yield return new WaitForSeconds(forgetPlayer);
        playerDetected = false;
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekPlayerCoroutine = SeekingPlayer();
        StartCoroutine(seekPlayerCoroutine);
    }

    //KK: Kod na dole dziala, jednak trzeba dobrze zaimplementowac to w StopFollowing();

    IEnumerator SeekingPlayer()
    {
        seekingActivated = false;
        Vector3 location = Vector3.zero;
        dialogue.Dialogue(SeekingPlayerDialogue[Random.Range(0, SeekingPlayerDialogue.Length)]);
        while(location!=transform.position)
        {
            location = transform.position;
            yield return new WaitForSeconds(3);
        }
        seekingActivated = true;
        float OriginalRotation = rb.rotation;
        float i = 0;
        while (true)
        {
            i = 0;
            while (i < 1)
            {
                rb.rotation = Mathf.LerpAngle(rb.rotation, OriginalRotation + 50, 0.03f);
                i += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            i = 0;
            while (i < 1)
            {
                rb.rotation = Mathf.LerpAngle(rb.rotation, OriginalRotation - 50, 0.03f);
                i += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
