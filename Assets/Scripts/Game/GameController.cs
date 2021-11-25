using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum State
{
    Default,
    Initial,
    Active,
    Paused,
    Death,
    Cleared,
    Shop,
    LevelUp,
    GameWin,
}

public class GameController : MonoBehaviour
{

    public State currentState;
    public State previousState;

    public bool stateChanged = false;

    public int totalCurrency = 0;

    public static GameController Instance { get; set; }

    [SerializeField] CurrencyUIManager currencyUI;
    public GameObject shopMenu;
    public GameObject pauseMenu;

    public GameObject levelUpMenu;
    public GameObject gameOverMenu;

    public GameObject gameWinMenu;


    [SerializeField] Text dungTextP1;
    [SerializeField] Text dungTextP2;

    [SerializeField] public Image dungBarP1;

    public List<Player> players;

    public float gameRuntime = 0f;

    public Text gameRuntimeText;
    public int delayAmount = 1; // Second count
    protected float Timer;

    PlayerInputManager playerInputManager;

    public AudioClip gameMusic;
    [SerializeField] AudioSource audioSource;


    private void Awake()
    {
        Instance = this;

        playerInputManager = GetComponent<PlayerInputManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(LevelManager.Instance.HandleLevelLoad(true));
    }

    public void StopGameMusic()
    {
        audioSource.Stop();
    }

    public void PlayGameMusic()
    {
        audioSource.Play();
    }

    private void Update()
    {

        // Handle state logic
        if (currentState != previousState)
        {

            if (currentState == State.Initial)
            {
                StartCoroutine(LevelTransition.Instance.OnStart());
            }

            if (currentState == State.Death)
            {
                StartCoroutine(LevelTransition.Instance.OnDeath());

            }

            if (currentState == State.GameWin)
            {
                StartCoroutine(LevelTransition.Instance.OnGameWin());

            }



            if (currentState == State.Active)
            {
                // PlayerInput playerInput = players[0].GetComponent<PlayerInput>();
                // PlayerInput levelupMenuInput = levelUpMenu.GetComponent<PlayerInput>();
                // playerInput.actions.Enable();
                // levelupMenuInput.actions.Disable();
                StartCoroutine(LevelTransition.Instance.OnUnpause());

            }

            if (currentState == State.Shop)
            {
                StartCoroutine(LevelTransition.Instance.OnEnterShop());
            }

            if (currentState == State.LevelUp)
            {
                // PlayerInput playerInput = players[0].GetComponent<PlayerInput>();
                // PlayerInput levelupMenuInput = levelUpMenu.GetComponent<PlayerInput>();
                // playerInput.actions.Disable();
                // levelupMenuInput.actions.Enable();

                StartCoroutine(LevelTransition.Instance.OnLevelUp());
            }

            previousState = currentState;
        }

        if (currentState == State.Active)
        {
            Timer += Time.deltaTime;

            if (Timer >= delayAmount)
            {
                Timer = 0f;
                gameRuntime++; // For every DelayAmount or "second" it will add one to the GoldValue
                float minutes = Mathf.Floor(gameRuntime / 60);
                float seconds = Mathf.Floor(gameRuntime) - (minutes * 60);

                gameRuntimeText.text = minutes.ToString() + "m " + seconds.ToString() + "s";
            }
        }
    }
    public void AddCurrency(int value)
    {
        totalCurrency += value;
        currencyUI.UpdateCurrency();
    }

    public void RemoveCurrency(int value)
    {
        totalCurrency -= value;
        currencyUI.UpdateCurrency();
    }

    public void SetDungText(float value, PlayerInput playerInput)
    {
        if (playerInput.playerIndex == 0)
        {
            dungTextP1.text = value.ToString("F0");
        }

        if (playerInput.playerIndex == 1)
        {
            dungTextP2.text = value.ToString("F0");
        }

    }
}
