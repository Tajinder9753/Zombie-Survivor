using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<Transform> transforms = new List<Transform>();
    [SerializeField] private int currentMaxEnemiesInLevel = 10;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private float intervalChange = 0.4f;
    [SerializeField] private int absoluteMaxEnemeies = 20;
    [SerializeField] private int chanceToSpawnIncrease = 10;
    private bool maxInterval;
    private bool maxEnemies;

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    //starts spawning enemies
    private IEnumerator StartSpawning()
    {
        while (true)
        {
            //spawns enemies until hits the max number allowed
            if (enemyCount < currentMaxEnemiesInLevel)
            {
                yield return new WaitForSeconds(spawnInterval);
                Enemy enemyToSpawn = PickEnemyToSpawn();
                Transform spawnPoint = PickSpawnPoint();
                Instantiate(enemyToSpawn, spawnPoint);
            }
            else
            {
                yield return null;
            }
        }

    }
    //selects an enemy from the list to spawn
    private Enemy PickEnemyToSpawn()
    {
        Enemy enemyToSpawn = enemies[0];
        int totalOdds = 0;
        foreach (Enemy enemy in enemies)
        {
            totalOdds += enemy.chanceToSpawn;
        }
        int randomNum = Random.Range(0, totalOdds);
        int cumulativeSum = 0;

        //uses cumulativeSum (weighted probability) to find the enemy to spawn
        foreach(Enemy enemy in enemies)
        {
            cumulativeSum += enemy.chanceToSpawn;
            if (randomNum < cumulativeSum)
            {
                return enemy;
            }
        }

        return enemyToSpawn; //fallback
    }

    //uses random range to find spawnpoint
    private Transform PickSpawnPoint()
    {
        int spawnPoint = Random.Range(0, transforms.Count);

        return transforms[spawnPoint];
    }

    //reduces enemyCount when enemy dies 
    public void EnemyDead()
    {
        enemyCount--;
    }

    //increases enemyCount when enemy spawns
    public void AddEnemy()
    {
        enemyCount++;
    }

    //increases the difficulty of the game when called in one of 3 ways
    public void IncreaseDifficulty()
    {
        //if already at the maxEnemies in level and the fastest spawn interval allowed then just change the enemySpawnChances for tougher enemies
        if (maxEnemies && maxInterval)
        {
            ChangeEnemyChances();
            return;
        }

        int randomNum = Random.Range(0, 2);
        int secondRandomNum = Random.Range(0, 1);

        switch (randomNum)
        {
            //if 0 should change the interval
            case 0: 
                if (!maxInterval)
                {
                    ChangeInterval();
                }
                //if already at the max interval use the second random num to determine if should increase the num of enemies or change the enemy chances
                else if (secondRandomNum == 0)
                {
                    IncreaseNumEnemies();
                }
                else
                {
                    ChangeEnemyChances();
                }
                break;
            case 1:
                if (!maxEnemies)
                {
                    IncreaseNumEnemies();
                }
                else if (secondRandomNum ==1)
                {
                    ChangeInterval();
                }
                else
                {
                    ChangeEnemyChances();
                }
                break;
            case 2:
                ChangeEnemyChances();
                break;
            default:
                break;
        }
    }

    private void ChangeInterval()
    {
        spawnInterval -= intervalChange;
        if (spawnInterval <= 1.0f) maxInterval = true;
    }

    private void IncreaseNumEnemies()
    {
        currentMaxEnemiesInLevel++;
        if (currentMaxEnemiesInLevel == absoluteMaxEnemeies) maxEnemies = true;
    }

    private void ChangeEnemyChances()
    {
        //increase the chance for tougher enemies (excluding the most basic enemy) to spawn
        int enemyToChange = Random.Range(1, enemies.Count);
        Enemy enemy = enemies[enemyToChange];
        enemy.chanceToSpawn += chanceToSpawnIncrease;
    }
}
