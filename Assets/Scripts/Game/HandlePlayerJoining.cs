using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerJoining : MonoBehaviour
{
    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.gameObject.GetComponent<PlayerMovement>() != null)
        {
            // LevelManager.Instance.UnpauseGame();
            Debug.Log("Player joined");
            Player newPlayer = playerInput.transform.GetComponent<Player>();
            GameController.Instance.currentState = State.Active;
            GameController.Instance.players.Add(newPlayer);
        }

    }

    void Update(){
        if(Touchscreen.current.primaryTouch.press.isPressed){
            Debug.Log("Touch");
        }
     
    }

}
