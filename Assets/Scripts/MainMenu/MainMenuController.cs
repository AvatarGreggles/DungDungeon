using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{

    [Header("Menu Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button nestButton;
    List<Button> buttons;

    [Header("Sound Effects")]
    [SerializeField] AudioClip switchItemSound;
    [SerializeField] AudioClip selectItemSound;
    AudioSource audioSource;

    PlayerInput playerInput;
    private Vector2 navigateMovement;

    int currentItemSelected = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = new List<Button>() { startButton, quitButton, nestButton };
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

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
        Application.Quit();
    }

    public void HandleNestClick(Button button = null)
    {
        SceneManager.LoadScene(2);
    }

    public void OnNavigateUI(InputValue value)
    {
        navigateMovement = value.Get<Vector2>();

        // Reset each buttons color to the default
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        // Handle navigating buttons
        if (navigateMovement.y > 0f)
        {
            PlaySwitchSound();
            currentItemSelected--;
        }
        else if (navigateMovement.y < 0f)
        {
            PlaySwitchSound();
            currentItemSelected++;
        }

        // Handles if the selection goes out of bounds and places it at start or end of the list
        if (currentItemSelected < 0)
        {
            currentItemSelected = buttons.Count - 1;
        }

        if (currentItemSelected > buttons.Count - 1)
        {
            currentItemSelected = 0;
        }

        // Set the buttons color as the selected color
        buttons[currentItemSelected].GetComponent<Image>().color = new Color(0.5f, 0.4f, 0.2f);
    }

    public void OnInteract()
    {
        // If no button is selected
        if (currentItemSelected == -1) { return; }

        PlaySelectSound();

        // Call the buttons method
        buttons[currentItemSelected].onClick.Invoke();
    }

    void PlaySwitchSound()
    {
        audioSource.PlayOneShot(switchItemSound, 1f);
    }

    void PlaySelectSound()
    {
        audioSource.PlayOneShot(switchItemSound, 1f);
    }
}
