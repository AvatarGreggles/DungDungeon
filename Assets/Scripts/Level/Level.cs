using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : ScriptableObject
{

    public GameObject listToSpawnFrom;
    public List<GameObject> levelListItems;

    [HideInInspector]
    public GameObject spawnedObject;

    public Vector3 prefabSpawnPoint;

    public enum LevelType
    {
        Level,
        Shop,
        Boss
    }

    public LevelType levelType;

    public GameObject GetRandomPrefabFromList()
    {
        List<GameObject> tempList = new List<GameObject>(); ;

        foreach (GameObject levelListItem in levelListItems)
        {
            if (levelListItem.activeSelf)
            {
                tempList.Add(levelListItem);
            }
        }


        int randomIndex = Random.Range(0, tempList.Count);
        //TODO: Make not same level load in a row
        if (tempList[randomIndex] != spawnedObject && tempList[randomIndex].activeSelf)
        {
            return tempList[randomIndex];
        }
        else
        {
            int newRandomIndex = Random.Range(0, tempList.Count);
            return tempList[newRandomIndex];
        }
    }

    public void LoadLevel()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("SpawnedObject");

        foreach (GameObject go in gos)
        {
            Destroy(go);
        }

        SpawnPrefab();
        if (levelType == LevelType.Shop)
        {
            LevelManager.Instance.EnterShop();
        }
        else if (levelType == LevelType.Boss)
        {
            LevelManager.Instance.EnterBossRoom();
        }
        else if (levelType == LevelType.Level)
        {
            LevelManager.Instance.NextLevel();
        }
    }


    public void SpawnPrefab()
    {
        levelListItems.Clear();
        foreach (Transform child in listToSpawnFrom.transform)
        {
            levelListItems.Add(child.gameObject);
        }

        spawnedObject = Instantiate(GetRandomPrefabFromList(), prefabSpawnPoint, Quaternion.identity);
        spawnedObject.transform.SetParent(GameObject.FindGameObjectWithTag("LevelStructure").transform);
    }
}