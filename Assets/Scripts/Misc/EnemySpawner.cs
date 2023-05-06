using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawners;
    [SerializeField] int minLvl = 0, maxLvl = 0, maxLvlOverall = 4, maxEnemies = 10;
    [SerializeField] GameObject EnemyPrefab, DreamyPrefab;

    bool canProgress = true;


    void Start()
    {
        StartCoroutine(SpawnerRoutine());
        StartCoroutine(LevelupRoutine());
    }

    void Update()
    {
        print(maxLvl);
    }

    IEnumerator SpawnerRoutine()
    {
        while (true)
        {
            if (GameObject.FindWithTag("Player").GetComponent<Player>().remainingEnemies < maxEnemies)
            {
                int randomMax = Random.Range(minLvl, maxLvl + 1);
                while (randomMax == 3) randomMax = Random.Range(minLvl, maxLvl + 1);
                int randomSpawner = Random.Range(0, enemySpawners.Length);
                if(Random.Range(0, 40) != 39)
                {
                    GameObject enemy = Instantiate(EnemyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                    enemy.transform.GetChild(0).rotation = enemySpawners[randomSpawner].rotation;
                    enemy.GetComponent<AIShooting>().equippedWeaponID = randomMax;
                    enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
                }
                else
                {
                    GameObject enemy = Instantiate(DreamyPrefab, enemySpawners[randomSpawner].position, Quaternion.identity);
                    enemy.transform.GetChild(0).rotation = enemySpawners[randomSpawner].rotation;
                    enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator LevelupRoutine()
    {
        while (canProgress)
        {
            yield return new WaitForSeconds(30f);
            /*if (minLvl < maxLvl)
            {
                minLvl++;
                if (minLvl == avoidLvl) minLvl++;
            }*/
            if (maxLvl < maxLvlOverall)
            {
                maxLvl++;
            }
            //if (maxLvl == maxLvlOverall) canProgress = false;
        }

        //yield return new WaitForSeconds(60f);
    }
}
