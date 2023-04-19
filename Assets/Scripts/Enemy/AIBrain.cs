using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Pathfinding;

public class AIBrain : MonoBehaviour
{
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] GameObject bloodPoolEffect;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject damagePopupPrefab;
    [SerializeField] GameObject moneyDropPrefab;
    [SerializeField] GameObject ammoDropPrefab;
    [SerializeField] GameObject medkitPrefab;
    [SerializeField] Rigidbody2D rb;

    private string[] SeekPlayerDialogue = { "Where are they?", "I lost them!", "I lost sight!", "Where did they hide?", "Dammit!", "They're gone!", "You can run,\nbut you can't hide!",
                                            "Come on, man!", "I can't take this shit\nno more man!", "Oh my lord!", "This is not\neven funny bro!", "I'm trying to get a\npayment check here!",
                                            "Huh?!", "They must have\nslipped away!", "I want to be dominated\nright now."};

    private string[] FoundPlayerDialogue = { "Here they are!", "Found them!", "You're going down!", "You're dead!", "You're screwed!", "Enemy spotted!",
                                             "Back by popular demand!", "What's the word,\nBruce?", "Yo yo yo!", "Dude stop!", "Yo, dude stop!", "Hump day!", 
                                             "Self pleasure does not\nmake you gay!", "I will knock you out!", "You can do\nthe thug shaker?", "Daddy, I'm coming!", "You ain't going anywhere,\nyou thick lump!" };

    private GameObject player;
    private GameObject damagePopup;
    private AIPath ai;

    float speed = 6f; 
    public int maxHealth = 100;

    public bool isStatic = false;
    public bool playerDetected = false;
    bool playerWasDetected = false;
    bool shotBefore = false;
    bool reacting = false;

    int currentHealth;

    float forgetPlayer = 3f;
    float forgetPlayerTimer;
    float notifyOthersCooldown = 1.5f;

    public int dropXP;
    public int dropMoney;
    public int dropHP;
    int summedDamage;
    float currentTime, currentTimeBloodPool;
    float reactionTime = 0.15f;
    float rotationSpeed = 0.25f;
    float aimAngle;

    RaycastHit hit;

    Vector3 aimDirection;
    Vector3 playerLocation;

    private IEnumerator stopFollowCoroutine;
    private IEnumerator seekPlayerCoroutine;
    private IEnumerator reactionCoroutine;
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

        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekPlayerCoroutine = SeekingPlayer();
        StartCoroutine(seekPlayerCoroutine);
    }

    void Update()
    {
        if(forgetPlayerTimer>0)forgetPlayerTimer -= Time.deltaTime;
        Colliders = Physics2D.OverlapCircleAll(transform.position, 12.5f);

        if (Colliders.Length > 0 && playerDetected && forgetPlayerTimer>notifyOthersCooldown)
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
        if (reactionCoroutine != null && !reacting) StopCoroutine(reactionCoroutine);
        if (!playerDetected && !reacting)
        {
            reactionCoroutine = ReactionTime();
            StartCoroutine(reactionCoroutine);
        }
        forgetPlayerTimer = forgetPlayer;
        stopFollowCoroutine = StopFollowing();
        StartCoroutine(stopFollowCoroutine);
    }

    public void TakeDamage(int damage)
    {
        if (!shotBefore)
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
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
            Destroy(healthBar.gameObject);
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
        healthBar.SetHealth(currentHealth);
        currentTime = Time.time;
    }

    IEnumerator ReactionTime()
    {
        reacting = true;
        yield return new WaitForSeconds(reactionTime);
        reacting = false;
        healthBar.Dialogue(FoundPlayerDialogue[Random.Range(0, FoundPlayerDialogue.Length)]);
        seekingActivated = false;
        playerDetected = true;
        playerWasDetected = true;
    }

    IEnumerator StopFollowing()
    {
        yield return new WaitForSeconds(forgetPlayer);
        playerDetected = false;
        healthBar.Dialogue(SeekPlayerDialogue[Random.Range(0, SeekPlayerDialogue.Length)]);
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekPlayerCoroutine = SeekingPlayer();
        StartCoroutine(seekPlayerCoroutine);
    }

    //KK: Kod na dole dziala, jednak trzeba dobrze zaimplementowac to w StopFollowing();

    IEnumerator SeekingPlayer()
    {
        seekingActivated = false;
        Vector3 location = Vector3.zero;
        while(location!=transform.position)
        {
            location = transform.position;
            yield return new WaitForSeconds(3);
        }
        seekingActivated = true;
        ai.destination = transform.position;
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
