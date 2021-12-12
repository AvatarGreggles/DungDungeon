using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{

    [Header("Menu Buttons")]
    [SerializeField] List<Button> buttons;
    [SerializeField] Color highlightedColor;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        // Any buttons add should get an on click listener and any accompanied method to be called
        buttons[0].onClick.AddListener(() =>
        {
            HandleStartClick();
        });

        buttons[1].onClick.AddListener(() =>
       {
           HandleQuitClick();
       });

        buttons[2].onClick.AddListener(() =>
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
        SetAsUnselected();

        // Handle navigating buttons
        HandleNavigation();

        // Handles if the selection goes out of bounds and places it at start or end of the list
        HandleMenuSelectionOutOfBounds();

        // Set the buttons color as the selected color
        SetAsSelected();
    }

    public void OnInteract()
    {
        // If no button is selected
        if (currentItemSelected == -1) { return; }

        PlaySelectSound();

        // Call the buttons method
        buttons[currentItemSelected].onClick.Invoke();
    }

    void HandleNavigation()
    {
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
    }

    void PlaySwitchSound()
    {
        audioSource.PlayOneShot(switchItemSound, 1f);
    }

    void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectItemSound, 1f);
    }

    void HandleMenuSelectionOutOfBounds()
    {
        if (currentItemSelected < 0)
        {
            currentItemSelected = buttons.Count - 1;
        }

        if (currentItemSelected > buttons.Count - 1)
        {
            currentItemSelected = 0;
        }
    }

    void SetAsSelected()
    {
        buttons[currentItemSelected].GetComponent<Image>().color = highlightedColor;
    }

    void SetAsUnselected()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }
}
