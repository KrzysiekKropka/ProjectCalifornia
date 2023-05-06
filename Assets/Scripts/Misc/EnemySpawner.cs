using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawners;
    bool dreamySpawn;
    int minLvl = 0, maxLvl = 0, maxLvlOverall = 0, maxEnemies = 0, currentLevel = 1, levels = 10;
    float spawnInterval;
    [SerializeField] GameObject EnemyPrefab, DreamyPrefab;

    bool canProgress = true;


    void Start()
    {
        StartCoroutine(SpawnerRoutine());
        StartCoroutine(LevelupRoutine());
        ChangeLevel();
    }

    void ChangeLevel()
    {
        GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>().SetLevel(currentLevel);

        switch(currentLevel)
        {
            case 1:
                minLvl = 0;
                maxLvl = 0;
                maxEnemies = 5;
                spawnInterval = 4f;
                dreamySpawn = false;
                break;
            case 2:
                minLvl = 0;
                maxLvl = 1;
                maxEnemies = 5;
                spawnInterval = 4f;
                dreamySpawn = false;
                break;
            case 3:
                minLvl = 0;
                maxLvl = 1;
                maxEnemies = 7;
                spawnInterval = 3.5f;
                dreamySpawn = false;
                break;
            case 4:
                minLvl = 0;
                maxLvl = 2;
                maxEnemies = 7;
                spawnInterval = 3.5f;
                dreamySpawn = false;
                break;
            case 5:
                minLvl = 0;
                maxLvl = 2;
                maxEnemies = 9;
                spawnInterval = 3f;
                dreamySpawn = true;
                break;
            case 6:
                minLvl = 0;
                maxLvl = 4;
                maxEnemies = 9;
                spawnInterval = 3f;
                dreamySpawn = true;
                break;
            case 7:
                minLvl = 0;
                maxLvl = 4;
                maxEnemies = 12;
                spawnInterval = 2.5f;
                dreamySpawn = true;
                break;
            case 8:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 12;
                spawnInterval = 2.5f;
                dreamySpawn = true;
                break;
            case 9:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 15;
                spawnInterval = 2f;
                dreamySpawn = true;
                break;
            case 10:
                minLvl = 1;
                maxLvl = 4;
                maxEnemies = 15;
                spawnInterval = 1.5f;
                dreamySpawn = true;
                break;
        }
    }

    IEnumerator SpawnerRoutine()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Player").GetComponent<Player>().remainingEnemies < maxEnemies)
            {
                int randomWeapon;

                if (minLvl != maxLvl) randomWeapon = Random.Range(minLvl, maxLvl + 1);
                else randomWeapon = minLvl;

                while (randomWeapon == 3) randomWeapon = Random.Range(minLvl, maxLvl+1);
                int randomSpawner = Random.Range(0, enemySpawners.Length);
                int randomChance = Random.Range(0, 50);

                if (randomChance == 49 && dreamySpawn)
                {
                    GameObject enemy = Instantiate(DreamyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                    enemy.transform.GetChild(0).rotation = enemySpawners[randomSpawner].rotation;
                    enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
                }
                else
                {
                    GameObject enemy = Instantiate(EnemyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                    enemy.transform.GetChild(0).rotation = enemySpawners[randomSpawner].rotation;
                    enemy.GetComponent<AIShooting>().equippedWeaponID = randomWeapon;
                    enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
                }
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

        //yield return new WaitForSeconds(60f);
        /*if (minLvl < maxLvl)
        {
            minLvl++;
            if (minLvl == avoidLvl) minLvl++;
        }*/
    }
}
