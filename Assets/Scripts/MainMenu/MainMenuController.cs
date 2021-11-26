using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [SerializeField] Button nestButton;

    [SerializeField] List<Button> buttons;

    PlayerInput playerInput;

    private Vector2 navigateMovement;

    int currentSkillSelected = -1;

    public AudioClip switchItemSound;
    public AudioClip selectItemSound;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    // IEnumerator EnterGame()
    // {
    //     var sequence = DOTween.Sequence();
    //     sequence.Append(Camera.main.transform.DOScaleZ(1f, 2.4f));
    //     yield return sequence.WaitForCompletion();
    // }

    public void HandleStartClick(Button button = null)
    {
        SceneManager.LoadScene(1);
        // StartCoroutine(EnterGame());
    }

    public void HandleQuitClick(Button button = null)
    {
        Debug.Log(button);
    }

    public void HandleNestClick(Button button = null)
    {
        SceneManager.LoadScene(2);
    }

    public void OnNavigateUI(InputValue value)
    {
        navigateMovement = value.Get<Vector2>();


        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        if (navigateMovement.y > 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentSkillSelected--;
        }
        else if (navigateMovement.y < 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentSkillSelected++;
        }

        if (currentSkillSelected < 0)
        {
            currentSkillSelected = buttons.Count - 1;
        }

        if (currentSkillSelected > buttons.Count - 1)
        {
            currentSkillSelected = 0;
        }

        Debug.Log(currentSkillSelected);

        buttons[currentSkillSelected].GetComponent<Image>().color = new Color(0.5f, 0.4f, 0.2f);
    }

    public void OnInteract()
    {
        if (currentSkillSelected == -1) { return; }
        audioSource.PlayOneShot(selectItemSound, 1f);
        buttons[currentSkillSelected].onClick.Invoke();
    }
}
