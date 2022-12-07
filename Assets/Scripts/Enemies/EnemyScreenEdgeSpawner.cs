using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScreenEdgeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;

     private void Start()
    {
      
    }

    private void Update()
    {
       // spawn an enemy ever x seconds

        if (Time.deltaTime % 5 == 0)
        {
            SpawnEnemy();
        }
        



    }

    void SpawnEnemy(){
          // Generate random x and y coordinates within the boundaries of the screen
        float x = Random.Range(0, Screen.width);
        float y = Random.Range(0, Screen.height);

        // Spawn a new enemy GameObject at the generated coordinates
        Instantiate(enemyToSpawn, new Vector2(x, y), Quaternion.identity);
    }
}
