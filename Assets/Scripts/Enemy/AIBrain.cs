using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIBrain : MonoBehaviour
{
    //Objects
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private AIShooting shootingScript;
    [SerializeField] private GameObject bloodPoolEffect;
    [SerializeField] private GameObject deadBody;
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private GameObject moneyDropPrefab;
    [SerializeField] private GameObject ammoDropPrefab;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] private Rigidbody2D rb;
    private GameObject player;
    private GameObject damagePopup;
    private AIPath ai;

    //Dialogues
    private string[] SeekPlayerDialogue = { "Where are they?", "I lost them!", "I lost sight!", "Where did they hide?", "Dammit!", "They're gone!", "You can run,\nbut you can't hide!",
                                            "Come on, man!", "I can't take this shit\nno more man!", "Oh my lord!", "This is not\neven funny bro!", "I'm trying to get a\npayment check here!",
                                            "Huh?!", "They must have\nslipped away!", "I want to be dominated\nright now."};

    private string[] FoundPlayerDialogue = { "Here they are!", "Found them!", "You're going down!", "You're dead!", "You're screwed!", "Enemy spotted!",
                                             "Back by popular demand!", "What's the word,\nBruce?", "Yo yo yo!", "Dude stop!", "Yo, dude stop!", "Hump day!", 
                                             "Self pleasure does not\nmake you gay!", "I will knock you out!", "You can do\nthe thug shaker?", "Daddy, I'm coming!", "You ain't going anywhere,\nyou thick lump!" };

    //Boss stuff
    public bool isDreamy = false;
    [SerializeField] private AudioClip DreamySeek, DreamyFind, DreamyDeath;
    public bool customHP = false;

    //Health
    private int currentHealth;
    public int maxHealth = 100;

    //Movement related
    [SerializeField] private float speed = 6f;
    public bool isStatic = false;

    //Player detection related
    public bool playerDetected = false;
    private bool playerWasDetected = false;
    private bool seekingActivated;
    private bool shotBefore = false;
    private bool reacting = false;
    private float forgetPlayer = 1.5f;
    private float forgetPlayerTimer;
    private float notifyOthersCooldown = 1f;
    private float reactionTime = 0.15f;
    private float rotationSpeed = 0.1f;
    [SerializeField] private float lockedRotationSpeed = 0.4f;
    private RaycastHit hit;
    private Vector3 playerLocation;

    //Dropped loot
    public int dropXP;
    public int dropMoney;
    public int dropHP;

    //Used for damage counter
    private int summedDamage;

    //Aiming related
    private float aimAngle;
    private Vector3 aimDirection;

    //IENumerators to store coroutines
    private IEnumerator stopFollowCoroutine;
    private IEnumerator seekPlayerCoroutine;
    private IEnumerator reactionCoroutine;

    //Colliders for nearby enemy detection
    private Collider2D[] Colliders;

    //Timers
    private float currentTime, currentTimeBloodPool;

    void Start()
    {
        deadBody.SetActive(false);
        ai = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)player.GetComponent<Player>().AddEnemy();
        ai.maxSpeed = speed;
        aimAngle = rb.rotation;

        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekPlayerCoroutine = SeekingPlayer();
        StartCoroutine(seekPlayerCoroutine);
    }

    void Update()
    {
        if(forgetPlayerTimer>0)forgetPlayerTimer -= Time.deltaTime;
        Colliders = Physics2D.OverlapCircleAll(transform.position, 7.5f);

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

        if (!isStatic && playerDetected) ai.destination = player.transform.position;
    }

    void FixedUpdate()
    {
        if (playerDetected) playerLocation = player.transform.position;
        if (playerLocation != null) aimDirection = playerLocation - transform.position;
        if (playerWasDetected) aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        if (!seekingActivated)
            rb.rotation = Mathf.LerpAngle(rb.rotation, aimAngle, rotationSpeed);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("PlayerBullet"))
        {
            PlayerInterrupts();
        }
    }

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

    public void ChangeAimLock(bool aimedAtPlayer = false)
    {
        if (aimedAtPlayer) rotationSpeed = lockedRotationSpeed;
        else rotationSpeed = 0.1f;
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

        if (damagePopup == null)
        {
            summedDamage = 0;
            damagePopup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            damagePopup.transform.position = transform.position;
        }

        summedDamage += damage;
        damagePopup.GetComponent<DamagePopup>().RestartAnim();
        damagePopup.GetComponent<DamagePopup>().SetDamageText(summedDamage);

        if (currentHealth <= 0)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            deadBody.SetActive(true);
            deadBody.GetComponent<DeadBodyFollow>().DeadBodyPosition(shootingScript.equippedWeaponID, isDreamy);
            shootingScript.enabled = false;
            Destroy(gameObject);
            Destroy(healthBar.gameObject);
            if (Time.time - currentTimeBloodPool > .01f)
            {
                if (player)
                {
                    player.SetXP(dropXP);
                    player.AddKill();
                    if (isDreamy) AudioSource.PlayClipAtPoint(DreamyDeath, transform.position);
                }
                Instantiate(bloodPoolEffect, transform.position, Quaternion.identity);
                GameObject droppedMoney = Instantiate(moneyDropPrefab, transform.position + new Vector3(-0.75f, -0.75f, 0f), Quaternion.identity);
                droppedMoney.GetComponent<MoneyDrop>().money = dropMoney;
                GameObject droppedAmmo = Instantiate(ammoDropPrefab, transform.position + new Vector3(0.75f, -0.75f, 0f), Quaternion.identity);
                GameObject droppedMedkit = Instantiate(medkitPrefab, transform.position + new Vector3(0f, 0.75f, 0f), Quaternion.identity);
                droppedMedkit.GetComponent<Medkit>().hp = dropHP;
                currentTimeBloodPool = Time.time;

                Destroy(droppedMoney, 60);
                Destroy(droppedAmmo, 60);
                Destroy(droppedMedkit, 60);
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
        if (!isDreamy) healthBar.Dialogue(FoundPlayerDialogue[Random.Range(0, FoundPlayerDialogue.Length)]);
        else AudioSource.PlayClipAtPoint(DreamyFind, transform.position);
        seekingActivated = false;
        playerDetected = true;
        playerWasDetected = true;
    }

    IEnumerator StopFollowing()
    {
        yield return new WaitForSeconds(forgetPlayer);
        playerDetected = false;
        ChangeAimLock(false);
        if (!isDreamy) healthBar.Dialogue(SeekPlayerDialogue[Random.Range(0, SeekPlayerDialogue.Length)]);
        else AudioSource.PlayClipAtPoint(DreamySeek, transform.position);
        if (seekPlayerCoroutine != null) StopCoroutine(seekPlayerCoroutine);
        seekPlayerCoroutine = SeekingPlayer();
        StartCoroutine(seekPlayerCoroutine);
    }

    IEnumerator SeekingPlayer()
    {
        seekingActivated = false;
        Vector3 location = Vector3.zero;
        while(location!=transform.position)
        {
            location = transform.position;
            yield return new WaitForSeconds(3f);
        }
        seekingActivated = true;
        ai.destination = transform.position;
        float OriginalRotation = rb.rotation;
        float i = 0;

        //KK: Ten kod po prostu obraca wroga kiedy "szuka" gracza
        while (true)
        {
            while (i < 1)
            {
                rb.rotation = Mathf.LerpAngle(rb.rotation, OriginalRotation + 50, 0.03f);
                i += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            i = 0;
            yield return new WaitForSeconds(1f);
            while (i < 1)
            {
                rb.rotation = Mathf.LerpAngle(rb.rotation, OriginalRotation - 50, 0.03f);
                i += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            i = 0;
            yield return new WaitForSeconds(1f);
        }
    }
}
