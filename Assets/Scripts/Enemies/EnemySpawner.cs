using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject Object;
    [SerializeField] float startTimeBtwSpawn;
    [SerializeField] int numberOfEnemiesToSpawn;
    int enemiesSpawned = 0;
    float timebtwspawn;

    [SerializeField] Transform spawnPos;



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
        GameObject enemy = Instantiate(Object, spawnPos.position, Quaternion.identity);
        enemiesSpawned++;
        timebtwspawn = startTimeBtwSpawn;
        LevelManager.Instance.PopulateEnemy(enemy);
    }

    void spawnTimeCountdown()
    {
        timebtwspawn -= Time.deltaTime;
    }
}
