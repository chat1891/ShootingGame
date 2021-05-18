using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;
    public Enemy enemy;

    int enemiesRemainingToSpawn;
    float nextSpawnTime;
    Wave currentWave;
    int CurrentWaveNumber;
    int EnemiesRemainingAlive;

    // Start is called before the first frame update
    void Start()
    {
        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpwans;

            Vector3 init = new Vector3(0, 1, 0);
            Enemy spawnedEnemy = Instantiate(enemy, init, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpwans;
    }

    void NextWave()
    {
        CurrentWaveNumber++;
        if (CurrentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[CurrentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            EnemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    void OnEnemyDeath()
    {
        EnemiesRemainingAlive--;
        if (EnemiesRemainingAlive <= 0)
        {
            NextWave();
        }
    }
}
