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

    private void Start()
    {
        DisableJoining();
        StartCoroutine(EnableJoinAfterIntro());
    }

    IEnumerator EnableJoinAfterIntro()
    {
        yield return new WaitForSeconds(1f);
        EnableJoining();

    }


    // Update is called once per frame
    void Update()
    {


        if (playerInputManager.playerCount == 1)
        {
            DisableJoining();
        }

        if ((playerInputManager.playerCount == playerInputManager.maxPlayerCount) || GameController.Instance.currentState == State.Death || GameController.Instance.currentState == State.GameWin)
        {
            DisableJoining();
        }

        // if (GameController.Instance.currentState == State.Death)
        // {
        //     DisableJoining();
        // }
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
