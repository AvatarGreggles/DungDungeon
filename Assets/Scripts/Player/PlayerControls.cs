using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    Collider2D collider;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        collider = GetComponent<Collider2D>();
    }


    public void OnPauseGame()
    {
        if (GameController.Instance.currentState == State.Initial || GameController.Instance.currentState == State.Death || GameController.Instance.currentState == State.GameWin || GameController.Instance.currentState == State.LevelUp || GameController.Instance.currentState == State.Shop)
        {
            return;
        }
        else
        {
            GameController.Instance.currentState = State.Paused;
        }


    }

    public void OnUnpauseGame()
    {
        GameController.Instance.currentState = State.Active;
    }

    public void OnToggleControls()
    {
        GameController.Instance.currencyUI.ToggleControls();
    }


    public void OnInteract()
    {

        if (GameController.Instance.currentState == State.Cleared)
        {
            InteractWithObject();
        }

        if (GameController.Instance.currentState == State.Dialog)
        {
            DialogManager.Instance.HandleNextLine();
        }
    }

    void InteractWithObject()
    {
        var facingDir = new Vector3(0f, 1f);
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 3f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            Debug.Log("something there");
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    public void OnCancel()
    {

        if (GameController.Instance.currentState == State.Shop)
        {
            GameController.Instance.currentState = State.Active;
            GameController.Instance.shopMenu.SetActive(false);
        }
    }
}
