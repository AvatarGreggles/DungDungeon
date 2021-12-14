using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();

    public Text enemiesLeftToKill;

    // Start is called before the first frame update

    [SerializeField] public int floor = 1;

    public static LevelManager Instance { get; set; }

    public DoorManager door;

    [SerializeField] Text floorText;

    [SerializeField] GameObject enemy;

    PlayerInputManager playerInputManager;

    [SerializeField] GameObject waitScreen;
    [SerializeField] public Transform playerSpawnPoint;
    [SerializeField] public List<Level> levels;

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
        if (floor >= levels.Count)
        {

            GameController.Instance.currentState = State.GameWin;
            yield break;
        }

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
        Shopkeeper shopkeeper = FindObjectOfType<Shopkeeper>();
        if (shopkeeper != null)
        {
            shopkeeper.hasVisited = false;
        }
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
        isFloorCleared = true;
        if (door != null)
        {
            door.ShowDoor();
        }

        GameController.Instance.currentState = State.Cleared;

        foreach (Player player in GameController.Instance.players)
        {
            PlayerLevelManager playerLevelManager = player.GetComponent<PlayerLevelManager>();
            if (player != null && player.isActiveAndEnabled)
            {
                playerLevelManager.MergeTempExperience();
            }

        }

        floorText.text = "Cleared";

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
        floorText.text = "Floor " + currentFloor;
    }

    public void PopulateEnemiesList()
    {
        foreach (GameObject enemyObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemyObj);
        }

        enemiesLeftToKill.text = enemies.Count.ToString();
    }

    public void PopulateEnemy(GameObject enemyObj)
    {
        enemies.Add(enemyObj);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        enemiesLeftToKill.text = enemies.Count.ToString();
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

    private void ResetShield()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in gos)
        {
            go.GetComponent<Player>().ResetShield();
            go.GetComponent<Player>().ResetInvincibility();
        }
    }

    public void NextLevel()
    {
        SetFloorText(floor.ToString());
        PopulateEnemiesList();
        ResetShooting();
        ResetShield();
    }


    public void EnterBossRoom()
    {
        SetFloorText("Boss");
        PopulateEnemiesList();
        ResetShooting();
        ResetShield();
    }

    public void EnterShop()
    {
        SetFloorText("Shop");
        ResetShield();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
