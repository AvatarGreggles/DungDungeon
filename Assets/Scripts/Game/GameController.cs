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
    Cleared,
    Shop,
    LevelUp,
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

    [SerializeField] Text dungTextP1;
    [SerializeField] Text dungTextP2;

    [SerializeField] public Image dungBarP1;

    public List<Player> players;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LevelManager.Instance.HandleLevelLoad(true));
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

            if (currentState == State.Paused)
            {
                StartCoroutine(LevelTransition.Instance.OnDeath());

            }

            if (currentState == State.Active)
            {
                StartCoroutine(LevelTransition.Instance.OnUnpause());
            }

            if (currentState == State.Shop)
            {
                StartCoroutine(LevelTransition.Instance.OnEnterShop());
            }

            if (currentState == State.LevelUp)
            {
                StartCoroutine(LevelTransition.Instance.OnLevelUp());
            }

            previousState = currentState;
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
