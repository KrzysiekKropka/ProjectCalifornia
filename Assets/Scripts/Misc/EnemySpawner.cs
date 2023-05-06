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

                if (randomChance == 49 && maxLvl >= 3)
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
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator LevelupRoutine()
    {
        while (canProgress)
        {
            yield return new WaitForSeconds(45f);
            if (maxLvl < maxLvlOverall)
            {
                maxLvl++;
            }
            if (maxLvl == maxLvlOverall) canProgress = false;
        }

        //yield return new WaitForSeconds(60f);
        /*if (minLvl < maxLvl)
        {
            minLvl++;
            if (minLvl == avoidLvl) minLvl++;
        }*/
    }
}
