using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            HandleStartClick(startButton);
        });

        quitButton.onClick.AddListener(() =>
       {
           HandleQuitClick(startButton);
       });
    }

    public void HandleStartClick(Button button = null)
    {
        SceneManager.LoadScene(1);
    }

    public void HandleQuitClick(Button button = null)
    {
        Debug.Log(button);
    }
}
