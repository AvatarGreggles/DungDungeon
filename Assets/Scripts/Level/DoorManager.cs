using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{

    PlayerLevelManager playerLevelManager;
    [SerializeField] TilemapRenderer tilemap;
    [SerializeField] TilemapCollider2D collider;

    public static DoorManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            playerLevelManager = player.GetComponent<PlayerLevelManager>();

            if (LevelManager.Instance.floor > LevelManager.Instance.levels.Count - 1)
            {
                GameController.Instance.currentState = State.GameWin;
                return;
            }

            if (playerLevelManager.willLevelUp)
            {
                GameController.Instance.currentState = State.LevelUp;
                playerLevelManager.ResetLevelUp();
            }
            else
            {
                MoveToNextLevel();
            }
        }
    }

    public void MoveToNextLevel()
    {
        StartCoroutine(LevelManager.Instance.HandleLevelLoad());
        ResetPlayers();
        HideDoor();
        DisableDoor();
    }

    void ResetPlayers()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in gos)
        {
            go.GetComponent<PlayerMovement>().ResetPosition();
            // go.GetComponent<Player>().ResetHealth();
            playerLevelManager.willLevelUp = false;
        }
    }

    public void HideDoor()
    {
        tilemap.enabled = true;
        collider.enabled = true;
    }

    public void ShowDoor()
    {
        tilemap.enabled = false;
        collider.enabled = false;
    }

    void DisableDoor()
    {
        gameObject.SetActive(false);
    }
}
