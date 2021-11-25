using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [SerializeField] Button nestButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            HandleStartClick();
        });

        quitButton.onClick.AddListener(() =>
       {
           HandleQuitClick();
       });

        nestButton.onClick.AddListener(() =>
       {
           HandleNestClick();
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

    public void HandleNestClick(Button button = null)
    {
        SceneManager.LoadScene(2);
    }
}
