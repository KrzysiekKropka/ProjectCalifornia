using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawners;
    bool dreamySpawn;
    int minLvl = 0, maxLvl = 0, maxEnemies = 0, currentLevel = 1, levels = 10;
    float spawnInterval;
    [SerializeField] GameObject EnemyPrefab, DreamyPrefab;
    private Player player;
    private HealthBar healthBar;

    bool canProgress = true;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>();
        StartCoroutine(SpawnerRoutine());
        StartCoroutine(LevelupRoutine());
        ChangeLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            canProgress = false;
            if (currentLevel < levels)
            {
                currentLevel++;
                ChangeLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            canProgress = false;
            if (currentLevel > 1)
            {
                currentLevel--;
                ChangeLevel();
            }
        }
    }

    void ChangeLevel()
    {
        healthBar.SetLevel(currentLevel);

        switch(currentLevel)
        {
            case 1:
                minLvl = 0;
                maxLvl = 0;
                maxEnemies = 2;
                spawnInterval = 4f;
                dreamySpawn = false;
                break;
            case 2:
                minLvl = 0;
                maxLvl = 1;
                maxEnemies = 2;
                spawnInterval = 4f;
                dreamySpawn = false;
                break;
            case 3:
                minLvl = 0;
                maxLvl = 1;
                maxEnemies = 4;
                spawnInterval = 4f;
                dreamySpawn = false;
                break;
            case 4:
                minLvl = 0;
                maxLvl = 2;
                maxEnemies = 4;
                spawnInterval = 3.5f;
                dreamySpawn = false;
                break;
            case 5:
                minLvl = 0;
                maxLvl = 2;
                maxEnemies = 6;
                spawnInterval = 3.5f;
                dreamySpawn = false;
                break;
            case 6:
                minLvl = 0;
                maxLvl = 4;
                maxEnemies = 6;
                spawnInterval = 3f;
                dreamySpawn = false;
                break;
            case 7:
                minLvl = 0;
                maxLvl = 4;
                maxEnemies = 8;
                spawnInterval = 3f;
                dreamySpawn = false;
                break;
            case 8:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 8;
                spawnInterval = 2.5f;
                dreamySpawn = true;
                break;
            case 9:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 10;
                spawnInterval = 2.5f;
                dreamySpawn = true;
                break;
            case 10:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 10;
                spawnInterval = 2f;
                dreamySpawn = true;
                break;
        }
    }

    IEnumerator SpawnerRoutine()
    {
        while (true)
        {
            if (player.remainingEnemies < maxEnemies)
            {
                int randomWeapon;

                if (minLvl != maxLvl) randomWeapon = Random.Range(minLvl, maxLvl + 1);
                else randomWeapon = minLvl;

                while (randomWeapon == 3) randomWeapon = Random.Range(minLvl, maxLvl+1);
                int randomSpawner = Random.Range(0, enemySpawners.Length);
                int randomChance = Random.Range(0, 40);

                GameObject enemy;

                if (randomChance == 39 && dreamySpawn)
                {
                    enemy = Instantiate(DreamyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                }
                else
                {
                    enemy = Instantiate(EnemyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                    enemy.GetComponent<AIShooting>().equippedWeaponID = randomWeapon;
                }
                enemy.transform.GetChild(0).GetComponent<AIBrain>().neverForget = true;
                enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator LevelupRoutine()
    {
        while (canProgress)
        {
            yield return new WaitForSeconds(30f);
            if (currentLevel < levels)
            {
                currentLevel++;
                ChangeLevel();
            }
            if (currentLevel == levels) canProgress = false;
        }
    }
}
