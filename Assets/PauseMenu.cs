using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{

    // render what runes and skills you had
    // [SerializeField] GameObject newSkillObject;
    // public List<Skill> skills;

    // [SerializeField] Transform skillList;


    [SerializeField] Text currencyText;

    [SerializeField] Text healthStatText;
    [SerializeField] Text shieldStatText;
    [SerializeField] Text attackStatText;
    [SerializeField] Text speedStatText;

    [SerializeField] Text critRatioStatText;

    [SerializeField] Text moneyEarnedText;
    [SerializeField] Text enemiesKilledText;

    [SerializeField] Text levelReachedText;
    [SerializeField] Text gameRuntimeText;
    Player player;
    [SerializeField] Button goToShopButton;

    [SerializeField] Button quitButton;

    [SerializeField] Button continueButton;

    [SerializeField] List<Button> buttons;

    int currentItemSelected = -1;

    private Vector2 navigateMovement;

    public AudioClip switchItemSound;
    public AudioClip selectItemSound;
    AudioSource audioSource;

    PlayerInput playerInput;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void UpdateCurrency()
    {
        currencyText.text = GameController.Instance.totalCurrency.ToString();
    }

    public void UpdateSpeedText()
    {
        speedStatText.text = player.attackSpeedBonus.ToString();
    }

    public void UpdateHPText()
    {
        healthStatText.text = player.health.ToString() + " / " + player.maxHealth.ToString();
    }

    public void UpdateGameRuntimeText()
    {
        float minutes = Mathf.Floor(GameController.Instance.gameRuntime / 60);
        float seconds = Mathf.Floor(GameController.Instance.gameRuntime) - (minutes * 60);

        gameRuntimeText.text = minutes.ToString() + "m " + seconds.ToString() + "s";
    }



    public void UpdateMoneyEarnedText()
    {
        moneyEarnedText.text = player.moneyEarned.ToString();
    }

    public void UpdateEnemiesKilledText()
    {
        enemiesKilledText.text = player.enemiesKilled.ToString();
    }

    public void UpdateLevelReachedText()
    {
        levelReachedText.text = player.levelReached.ToString();
    }

    public void UpdateShieldText()
    {
        shieldStatText.text = player.shield.ToString() + " / " + player.maxShield.ToString();
    }

    public void UpdateAttackText()
    {
        attackStatText.text = player.attack.ToString();
    }

    public void UpdateCritRatioText()
    {
        critRatioStatText.text = player.criticalHitRatio.ToString();
    }

    private void OnEnable()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void UnpauseGame()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");
        StartCoroutine(LevelTransition.Instance.OnUnpause());
    }


    // Start is called before the first frame update
    void Start()
    {

        goToShopButton.onClick.AddListener(() =>
{
    HandleGoToShop();
});

        quitButton.onClick.AddListener(() =>
     {
         HandleGoToMainMenu();
     });

        continueButton.onClick.AddListener(() =>
      {
          HandleContinueGame();
      });


        player = GameController.Instance.players[0];

        player.GetComponent<PlayerInput>().enabled = false;

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();
        UpdateCritRatioText();
        UpdateEnemiesKilledText();
        UpdateLevelReachedText();
        UpdateMoneyEarnedText();
        UpdateGameRuntimeText();
    }

    public void HandleGoToShop()
    {
        SceneManager.LoadScene(2);
    }

    public void HandleGoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void HandleContinueGame()
    {
        UnpauseGame();
    }

    public void OnNavigateUI(InputValue value)
    {
        navigateMovement = value.Get<Vector2>();


        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        if (currentItemSelected == -1)
        {
            currentItemSelected = 0;
            audioSource.PlayOneShot(switchItemSound, 1f);
            return;
        }

        // On top row


        if (navigateMovement.x > 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentItemSelected++;
        }
        else if (navigateMovement.x < 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentItemSelected--;
        }

        // Move to next section
        if (navigateMovement.y > 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentItemSelected--;
        }
        else if (navigateMovement.y < 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentItemSelected++;
        }



        if (currentItemSelected < 0)
        {
            currentItemSelected = buttons.Count - 1;
        }

        if (currentItemSelected > buttons.Count - 1)
        {
            currentItemSelected = 0;
        }


        buttons[currentItemSelected].GetComponent<Image>().color = new Color(0.5f, 0.4f, 0.2f);
    }

    public void OnInteract()
    {
        if (currentItemSelected == -1) { return; }
        audioSource.PlayOneShot(selectItemSound, 1f);
        player.GetComponent<PlayerInput>().enabled = true;
        buttons[currentItemSelected].onClick.Invoke();
        currentItemSelected = -1;
    }
}
