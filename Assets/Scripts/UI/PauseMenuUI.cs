using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenuUI : MonoBehaviour
{

    [SerializeField] Button unpauseButton;


    private void Start()
    {

        unpauseButton.onClick.AddListener(() =>
     {
         HandleUnpauseClick(unpauseButton);
     });
    }

    private void OnEnable()
    {
        Debug.Log("hello");
    }

    public void HandleUnpauseClick(Button button = null)
    {
        GameController.Instance.currentState = State.Active;
    }

    // Start is called before the first frame update [SerializeField] Text currencyText;
}
