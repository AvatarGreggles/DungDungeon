using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update

    [SerializeField] public int floor = 1;

    public static LevelManager Instance { get; set; }

    [SerializeField] DoorManager door;

    [SerializeField] Text floorText;

    [SerializeField] GameObject enemy;

    PlayerInputManager playerInputManager;

    [SerializeField] GameObject waitScreen;
    [SerializeField] public Transform playerSpawnPoint;
    [SerializeField] List<Level> levels;

    bool isFloorCleared = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }


    public IEnumerator HandleLevelLoad(bool isFirstLevelLoad = false)
    {
        if (isFirstLevelLoad)
        {
            GameController.Instance.currentState = State.Initial;
            // StartCoroutine(LevelTransition.Instance.OnLevelChange());
        }

        if (!isFirstLevelLoad)
        {
            IncrementFloor(1);
        }
        levels[floor - 1].LoadLevel();
        SetPlayerSpawnPoint();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInputManager.playerCount > 0)
        {
            if (AreAllEnemiesDead() && !isFloorCleared)
            {
                OnFloorCleared();
            }
        }
    }

    bool AreAllEnemiesDead()
    {
        if (enemies.Count <= 0)
        {
            return true;
        }

        return false;
    }

    private void OnFloorCleared()
    {
        door.gameObject.SetActive(true);
        isFloorCleared = true;
        GameController.Instance.currentState = State.Cleared;

        foreach (Player player in GameController.Instance.players)
        {
            if (player.isActiveAndEnabled)
            {
                player.MergeTempExperience();
            }

        }

    }

    private void IncrementFloor(int floorsToIncrement)
    {
        floor += floorsToIncrement;
        // GameController.Instance.currentState = State.Paused;
        StartCoroutine(LevelTransition.Instance.OnLevelChange());
        isFloorCleared = false;
    }

    private void SetFloorText(string currentFloor)
    {
        floorText.text = currentFloor;
    }

    private void PopulateEnemiesList()
    {
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemyObj);
        }
    }

    private void ResetShooting()
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in gos)
        {
            SetPlayerSpawnPoint();
            go.SetActive(true);
            if (go.GetComponent<PlayerAttack>() != null)
            {
                // StartCoroutine(go.GetComponent<PlayerAttack>().Shooting());
            }
        }

    }

    void SetPlayerSpawnPoint()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
        foreach (GameObject spawnPoint in spawnPoints)
        {
            if (spawnPoint.activeSelf)
            {
                playerSpawnPoint = spawnPoint.transform;
            }
        }
    }

    public void NextLevel()
    {
        SetFloorText(floor.ToString());
        PopulateEnemiesList();
        ResetShooting();
    }


    public void EnterBossRoom()
    {
        SetFloorText("Boss");
        PopulateEnemiesList();
        ResetShooting();
    }

    public void EnterShop()
    {
        SetFloorText("Shop");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
