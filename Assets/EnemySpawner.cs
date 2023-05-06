using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] enemySpawners;
    [SerializeField] int minLvl = 0, maxLvl = 0, maxLvlOverall = 4, maxEnemies = 10;
    [SerializeField] GameObject EnemyPrefab;

    bool canProgress = true;


    void Start()
    {
        StartCoroutine(SpawnerRoutine());
        StartCoroutine(LevelupRoutine());
    }

    IEnumerator SpawnerRoutine()
    {
        while(true)
        {
            if(GameObject.FindWithTag("Player").GetComponent<Player>().remainingEnemies < maxEnemies)
            {
                int randomMax = Random.Range(minLvl, maxLvl);
                while (randomMax == 3) randomMax = Random.Range(minLvl, maxLvl);
                GameObject enemy = Instantiate(EnemyPrefab, enemySpawners[Random.Range(0, enemySpawners.Length)].position, Quaternion.identity);
                enemy.GetComponent<AIShooting>().equippedWeaponID = randomMax;
                enemy.transform.GetChild(0).GetComponent<AIBrain>().PlayerInterrupts();
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
