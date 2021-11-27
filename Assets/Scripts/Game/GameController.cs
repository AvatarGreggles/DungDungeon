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

public class GameController : MonoBehaviour, ISavable
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

    public void LoadData()
    {
        SavingSystem.i.Load("saveSlot1");

        currencyUI.UpdateCurrency();

        float minutes = Mathf.Floor(gameRuntime / 60);
        float seconds = Mathf.Floor(gameRuntime) - (minutes * 60);

        gameRuntimeText.text = minutes.ToString() + "m " + seconds.ToString() + "s";
    }

    public void SaveData()
    {
        SavingSystem.i.Save("saveSlot1");
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
                // SavingSystem.i.Save("saveSlot1");
                SaveData();
                StartCoroutine(LevelTransition.Instance.OnDeath());

            }

            if (currentState == State.Paused)
            {
                // SavingSystem.i.Save("saveSlot1");
                SaveData();
                StartCoroutine(LevelTransition.Instance.OnPause());

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

    public object CaptureState()
    {
        Player player = players[0].GetComponent<Player>();
        var saveState = new GameControllerSaveState()
        {
            totalCurrentCurrency = totalCurrency,
            totalGameRuntime = gameRuntime,

            playerHealth = player.health,
            playerExperience = player.experience,
            playerTemporaryExperienceHolder = player.temporaryExperienceHolder,
            playerExperienceToNextLevel = player.experienceToNextLevel,
            playerLevel = player.level,
            playerPreviousLevel = player.previousLevel,
            playerCriticalHitRatio = player.criticalHitRatio,
            playerMaxHealth = player.maxHealth,
            playerAttackSpeedBonus = player.attackSpeedBonus,
            playerShield = player.shield,
            playerMaxShield = player.maxShield,
            // playerHealthBar = player.healthBar,
            // playerShieldBar = player.shieldBar,
            // playerExpBar = player.expBar,
            playerAttack = player.attack,
            playerDungAccumulated = player.dungAccumulated,
            playerPrevDungAccumulated = player.prevDungAccumulated,
            playerMaxDungSize = player.maxDungSize,
            playerToLevelUp = player.toLevelUp,
            playerLevelReached = player.levelReached,
            playerEnemiesKilled = player.enemiesKilled,
            playerMoneyEarned = player.moneyEarned,
            playerInvincibilityFrameTime = player.invincibilityFrameTime,
            playerIsInvincible = player.isInvincible,
            playerWillLevelUp = player.willLevelUp,

        };
        Debug.Log("Game controller saved");

        return saveState;
    }

    public void RestoreState(object state)
    {
        Player player = players[0].GetComponent<Player>();
        GameControllerSaveState loadedData = (GameControllerSaveState)state;

        totalCurrency = loadedData.totalCurrentCurrency;
        gameRuntime = loadedData.totalGameRuntime;

        if (player != null)
        {
            player.health = loadedData.playerHealth;
            player.experience = loadedData.playerExperience;
            player.temporaryExperienceHolder = loadedData.playerTemporaryExperienceHolder;
            player.experienceToNextLevel = loadedData.playerExperienceToNextLevel;
            player.level = loadedData.playerLevel;
            player.previousLevel = loadedData.playerPreviousLevel;
            player.criticalHitRatio = loadedData.playerCriticalHitRatio;
            player.maxHealth = loadedData.playerMaxHealth;
            player.attackSpeedBonus = loadedData.playerAttackSpeedBonus;
            player.shield = loadedData.playerShield;
            player.maxShield = loadedData.playerMaxShield;
            // player.healthBar = loadedData.playerHealthBar;
            // player.shieldBar = loadedData.playerShieldBar;
            // player.expBar = loadedData.playerExpBar;
            player.attack = loadedData.playerAttack;
            player.dungAccumulated = loadedData.playerDungAccumulated;
            player.prevDungAccumulated = loadedData.playerPrevDungAccumulated;
            player.maxDungSize = loadedData.playerMaxDungSize;
            player.toLevelUp = loadedData.playerToLevelUp;
            player.levelReached = loadedData.playerLevelReached;
            player.enemiesKilled = loadedData.playerEnemiesKilled;
            player.moneyEarned = loadedData.playerMoneyEarned;
            player.invincibilityFrameTime = loadedData.playerInvincibilityFrameTime;
            player.isInvincible = loadedData.playerIsInvincible;
            player.willLevelUp = loadedData.playerWillLevelUp;

            Debug.Log("Player data loaded");
        }
        Debug.Log("Game controller loaded");
    }

    [System.Serializable]
    public class GameControllerSaveState
    {
        public int totalCurrentCurrency;
        public float totalGameRuntime;

        public float playerHealth;
        public float playerExperience;
        public float playerExperienceToNextLevel;
        public float playerTemporaryExperienceHolder;
        public int playerLevel;
        public int playerPreviousLevel;
        public float playerCriticalHitRatio;
        public float playerMaxHealth;
        public float playerAttackSpeedBonus;
        public float playerShield;
        public float playerMaxShield;
        // public GameObject playerHealthBar;
        // public GameObject playerShieldBar;
        // public GameObject playerExpBar;
        public float playerAttack;
        public float playerDungAccumulated;
        public float playerPrevDungAccumulated;
        public float playerMaxDungSize;
        public int[] playerToLevelUp;
        public int playerLevelReached;
        public int playerEnemiesKilled;
        public int playerMoneyEarned;
        public float playerInvincibilityFrameTime;
        public bool playerIsInvincible;
        public bool playerWillLevelUp;


    }
}