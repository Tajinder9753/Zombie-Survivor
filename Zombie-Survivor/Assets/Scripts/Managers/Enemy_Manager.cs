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

    public void AddEnemy()
    {
        enemyCount++;
    }

    public void IncreaseDifficulty()
    {
        if (maxEnemies && maxInterval)
        {
            ChangeEnemyChances();
            return;
        }

        int randomNum = Random.Range(0, 2);
        int secondRandomNum = Random.Range(0, 1);

        switch (randomNum)
        {
            case 0: 
                if (!maxInterval)
                {
                    ChangeInterval();
                }
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
        int enemyToChange = Random.Range(1, enemies.Count);
        Enemy enemy = enemies[enemyToChange];
        enemy.chanceToSpawn += chanceToSpawnIncrease;
    }
}
