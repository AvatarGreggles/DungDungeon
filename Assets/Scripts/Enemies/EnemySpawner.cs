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
        if (timebtwspawn <= 0 && enemiesSpawned < numberOfEnemiesToSpawn && GameController.Instance.currentState == State.Active)
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
        enemy.GetComponent<Enemy>().isSpawnee = true;
        LevelManager.Instance.PopulateEnemy(enemy);
        enemiesSpawned++;
        timebtwspawn = startTimeBtwSpawn;
    }

    void spawnTimeCountdown()
    {
        timebtwspawn -= Time.deltaTime;
    }
}
