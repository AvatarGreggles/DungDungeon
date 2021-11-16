using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleGlobalPlayerInput : MonoBehaviour
{

    PlayerInputManager playerInputManager;
    bool maxPlayerLimitReached = false;

    public static HandleGlobalPlayerInput Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        // if ((playerInputManager.playerCount == playerInputManager.maxPlayerCount && !maxPlayerLimitReached) || GameController.Instance.currentState == State.Paused)
        // {
        //     DisableJoining();
        // }

        if (GameController.Instance.currentState == State.Active)
        {
            EnableJoining();
        }
    }

    void DisableJoining()
    {
        playerInputManager.DisableJoining();
        maxPlayerLimitReached = true;
    }

    void EnableJoining()
    {
        playerInputManager.EnableJoining();
        maxPlayerLimitReached = true;
    }

    public void JoinPlayer()
    {
        // Debug.Log("joined");
        // playerInputManager.JoinPlayer();
    }


}
