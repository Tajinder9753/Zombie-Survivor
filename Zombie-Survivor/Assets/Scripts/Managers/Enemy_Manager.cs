using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<Transform> transforms = new List<Transform>();
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private float spawnInterval = 5f;
    private int enemyCount = 0;

    private void Update()
    {
        if (enemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        StartCoroutine(StartSpawning());
    }

    //starts spawning enemies
    private IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(spawnInterval);
        Enemy enemyToSpawn = PickEnemyToSpawn();
        Transform spawnPoint = PickSpawnPoint();
        Instantiate(enemyToSpawn, spawnPoint);
        enemyCount++;

    }
    //selects an enemy from the list to spawn
    private Enemy PickEnemyToSpawn()
    {
        return enemies[enemyCount - 1];
    }

    private Transform PickSpawnPoint()
    {
        return transforms[enemyCount - 1];
    }

    //reduces enemyCount when enemy dies 
    public void EnemyDead()
    {
        enemyCount--;
    }
}
