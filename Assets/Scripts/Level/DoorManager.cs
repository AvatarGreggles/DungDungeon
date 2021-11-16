using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{

    [SerializeField] TilemapRenderer tilemap;

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
            StartCoroutine(LevelManager.Instance.HandleLevelLoad());
            ResetPlayers();
            HideDoor();
            DisableDoor();
        }
    }

    void ResetPlayers()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject go in gos)
        {
            go.GetComponent<PlayerMovement>().ResetPosition();
            go.GetComponent<Player>().ResetHealth();
        }
    }

    void HideDoor()
    {
        tilemap.enabled = true;
    }

    void ShowDoor()
    {
        tilemap.enabled = false;
    }

    void DisableDoor()
    {
        gameObject.SetActive(false);
    }
}
