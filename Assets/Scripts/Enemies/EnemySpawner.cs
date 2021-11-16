using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject Object;
    [SerializeField] float startTimeBtwSpawn;
    [SerializeField] int numberOfEnemiesToSpawn;
    int enemiesSpawned = 0;
    float timebtwspawn;

    void Update()
    {
        if (timebtwspawn <= 0 && enemiesSpawned < numberOfEnemiesToSpawn)
        {
            SpawnEnemy();
        }
        else
        {
            spawnTimeCountdown();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(Object, transform.position, Quaternion.identity);
        enemiesSpawned++;
        timebtwspawn = startTimeBtwSpawn;
    }

    void spawnTimeCountdown()
    {
        timebtwspawn -= Time.deltaTime;
    }
}
