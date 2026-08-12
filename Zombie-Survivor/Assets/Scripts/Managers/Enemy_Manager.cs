using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<Transform> transforms = new List<Transform>();
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int enemyCount = 0;

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    //starts spawning enemies
    private IEnumerator StartSpawning()
    {
        while (true)
        {
            if (enemyCount < maxEnemies)
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

    public void IncreaseChances()
    {

    }
}
