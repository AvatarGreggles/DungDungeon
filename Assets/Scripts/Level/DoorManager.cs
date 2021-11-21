using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{

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
        ShowDoor();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            ShowDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player.willLevelUp)
            {
                GameController.Instance.currentState = State.LevelUp;
                player.previousLevel = player.level;
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
            go.GetComponent<Player>().ResetHealth();
            go.GetComponent<Player>().willLevelUp = false;
        }
    }

    void HideDoor()
    {
        tilemap.enabled = true;
        collider.enabled = true;
    }

    void ShowDoor()
    {
        tilemap.enabled = false;
        collider.enabled = false;
    }

    void DisableDoor()
    {
        gameObject.SetActive(false);
    }
}
