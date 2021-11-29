using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;


public class Player : MonoBehaviour
{

    public float health;

    public float experience;

    public float baseXP;
    public float temporaryExperienceHolder;
    public float experienceToNextLevel;
    public int level;
    public int previousLevel;

    public float criticalHitRatio = 6.25f;

    [SerializeField] public float maxHealth = 3;

    public float attackSpeedBonus;

    public float shield;
    [SerializeField] public float maxShield = 3;
    [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject shieldBar;
    [SerializeField] public GameObject expBar;

    public float attack = 1;
    public GameObject damageDisplayPivot;

    [SerializeField] GameObject p1Tag;
    [SerializeField] GameObject p2Tag;

    [SerializeField] PlayerInput playerInput;

    public AudioClip hurtSound;
    AudioSource audioSource;

    Vector3 initialHealthBarSize;
    Vector3 initialShieldBarSize;
    Vector3 initialEXPBarSize;
    float initialDungBarSize;

    public SpriteRenderer dungSprite;

    public float dungAccumulated = 0f;
    public float prevDungAccumulated = 0f;

    public float maxDungSize = 4f;

    public bool isShooting = false;

    public int[] toLevelUp = new int[1];

    public int levelReached = 0;
    public int enemiesKilled = 0;
    public int moneyEarned = 0;

    Collider2D collider;

    public float invincibilityFrameTime = 4f;
    public bool isInvincible = false;

    public bool willLevelUp = false;


    [SerializeField] TMPro.TMP_Text playerLevelTextP1;
    // [SerializeField] Text playerLevelTextP2;

    public PlayerAbilities playerAbilities;

    public int delayAmount = 10; // Second count
    protected float Timer;

    PlayerBaseStatManager playerBaseStats;

    [SerializeField] GameObject criticalDamageSprite;
    [SerializeField] GameObject damageSprite;

    [SerializeField] public List<Item> itemInventory;
    [SerializeField] public List<Skill> skillInventory;

    float stopwatch = 0;


    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
        collider = GetComponent<Collider2D>();
        playerAbilities = GetComponent<PlayerAbilities>();

        GameController.Instance.LoadData();

    }


    void Start()
    {
        playerBaseStats = FindObjectOfType<PlayerBaseStatManager>();
        maxHealth = maxHealth + playerBaseStats.bonusMaxHP;
        health = maxHealth;
        maxShield = maxShield + playerBaseStats.bonusMaxShield;
        shield = maxShield;
        attack = attack + playerBaseStats.bonusAttackPower;
        maxDungSize = maxDungSize + playerBaseStats.bonusMaxDung;

        previousLevel = level;
        SetPlayerTag();
        initialHealthBarSize = healthBar.transform.localScale;
        initialShieldBarSize = shieldBar.transform.localScale;
        initialEXPBarSize = expBar.transform.localScale;
        initialDungBarSize = GameController.Instance.dungBarP1.fillAmount;

        SetLevelText();
        LevelXPSetUp();
        expBar.transform.localScale = new Vector3(initialEXPBarSize.x * (experience / toLevelUp[level]), initialEXPBarSize.y, initialEXPBarSize.z);
        GameController.Instance.dungBarP1.fillAmount = dungAccumulated / maxDungSize;
        dungSprite.enabled = false;

        // GameController.Instance.LoadData();

    }

    private void Update()
    {

        if (isInvincible == true)
        {
            stopwatch += Time.deltaTime;
            if (stopwatch >= invincibilityFrameTime)
            {
                Debug.Log("no longer inv");
                isInvincible = false;
            }
        }
        else
        {
            stopwatch = 0;
        }

        if (dungAccumulated != prevDungAccumulated && dungAccumulated > 0 && dungAccumulated < maxDungSize && !isShooting)
        {
            SetSpriteSize();
            prevDungAccumulated = dungAccumulated;
        }

        if (playerAbilities.hpRegeneration)
        {
            Timer += Time.deltaTime;

            if (Timer >= delayAmount)
            {
                Timer = 0f;
                if (health < maxHealth)
                {
                    float amountToHeal = (maxHealth / 100) * 5;
                    health = health + amountToHeal;
                    UpdateHealthBar();
                }

                if (health == maxHealth)
                {
                    UpdateHealthBar();
                }

            }
        }
    }

    private void SetSpriteSize()
    {
        ShowSprite();
        dungSprite.transform.localScale += new Vector3(0.025f, 0.025f, 0f);
    }

    public void AccumulateDung(float dungAccumulationRate)
    {
        dungAccumulated += dungAccumulationRate;
        GameController.Instance.dungBarP1.fillAmount = dungAccumulated / maxDungSize;
        GameController.Instance.SetDungText(dungAccumulated, playerInput);
    }

    public void GainLevel()
    {
        level += 1;
        levelReached = level;
        SetLevelText();
    }

    public void AddItem(Item item)
    {
        itemInventory.Add(item);
    }

    public void AddSkill(Skill skill)
    {
        skillInventory.Add(skill);
    }

    public void OnPauseGame()
    {
        if (GameController.Instance.currentState == State.Initial || GameController.Instance.currentState == State.Death || GameController.Instance.currentState == State.GameWin || GameController.Instance.currentState == State.LevelUp || GameController.Instance.currentState == State.Shop)
        {
            return;
        }
        else
        {
            Debug.Log("Is clicked");
            GameController.Instance.currentState = State.Paused;
        }


    }

    public void OnUnpauseGame()
    {
        GameController.Instance.currentState = State.Active;
    }

    void SetLevelText()
    {
        if (playerInput.playerIndex == 0)
        {
            playerLevelTextP1.text = level.ToString();
        }
    }

    void LevelXPSetUp()
    {
        for (int i = 1; i < toLevelUp.Length; i++)
        {
            toLevelUp[i] = (int)(Mathf.Floor(baseXP * (Mathf.Pow(i, 1.2f))));
        }
    }

    //should count up until it hits the experience amount to add.
    public IEnumerator FillExperienceBar(float experienceToAdd)
    {
        if (level < toLevelUp.Length)
        {

            temporaryExperienceHolder = 0;

            if (experience + experienceToAdd >= toLevelUp[level])
            {
                willLevelUp = true;
            }

            // addingXp = true;
            //received from external sources. Add xp incrementally to move bar up slowly instead of chunks.
            for (int i = 0; i < experienceToAdd; i++)
            {
                if (level < toLevelUp.Length)
                {
                    Debug.Log(level);
                    experience++;
                    expBar.transform.localScale = new Vector3(initialEXPBarSize.x * (experience / toLevelUp[level]), initialEXPBarSize.y, initialEXPBarSize.z);



                    if (experience >= toLevelUp[level])
                    {
                        GainLevel();
                        if (level < toLevelUp.Length)
                        {
                            experience = toLevelUp[level - 1] - experience;
                            if (experience < 0)
                            {
                                experience *= 1;
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(.001f);
            }

            // addingXp = false;
        }
    }

    public void ResetShield()
    {
        shield = maxShield;
        shieldBar.transform.localScale = new Vector3(initialShieldBarSize.x * (shield / maxShield), initialShieldBarSize.y, initialShieldBarSize.z);

    }

    public void ResetInvincibility()
    {
        isInvincible = false;
    }

    public void MergeTempExperience()
    {
        StartCoroutine(FillExperienceBar(temporaryExperienceHolder));
        // GainEXP(temporaryExperienceHolder);
    }

    //TODO: Reaname this method
    public void UpdateTempExperienceHolder(float value, int currency)
    {
        moneyEarned += currency;
        enemiesKilled++;
        temporaryExperienceHolder += value;
    }


    public void ResetSpriteSize()
    {
        HideSprite();
        dungSprite.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void HideSprite()
    {
        dungSprite.enabled = false;
    }

    public void ShowSprite()
    {
        dungSprite.enabled = true;
    }

    void SetPlayerTag()
    {
        if (playerInput.playerIndex == 0)
        {
            p1Tag.SetActive(true);
        }

        if (playerInput.playerIndex == 1)
        {
            p2Tag.SetActive(true);
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(initialHealthBarSize.x * (health / maxHealth), initialHealthBarSize.y, initialHealthBarSize.z);
    }

    public void OnNavigateUI(InputValue value)
    {


    }

    public void OnInteract()
    {


    }

    public void OnCancel()
    {
        Debug.Log("Cancelling");

        if (GameController.Instance.currentState == State.Shop)
        {
            GameController.Instance.currentState = State.Active;
            GameController.Instance.shopMenu.SetActive(false);
            // playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void DealDamage(int damage, bool isCriticalHit)
    {
        if (isInvincible) { return; }
        isInvincible = true;

        GameObject spriteToInstantiate = isCriticalHit ? criticalDamageSprite : damageSprite;
        GameObject damageObject = Instantiate(spriteToInstantiate, damageDisplayPivot.transform.position, damageDisplayPivot.transform.rotation);
        damageObject.transform.SetParent(transform);
        damageObject.GetComponent<DisplayDamage>().showDamage(damage);
        damageObject.transform.SetParent(null);

        if (shield > 0)
        {
            shield -= damage;

            if (shield < 0)
            {
                shield = 0;
            }

            shieldBar.transform.localScale = new Vector3(initialShieldBarSize.x * (shield / maxShield), initialShieldBarSize.y, initialShieldBarSize.z);

        }
        else
        {

            shieldBar.transform.localScale = new Vector3(0f, initialShieldBarSize.y, initialShieldBarSize.z);
            health -= damage;

            UpdateHealthBar();

        }
        if (gameObject != null)
        {
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation(gameObject));
        }

        audioSource.PlayOneShot(hurtSound, 1f);

        IsPlayerDead();

        if (AreAllPlayersDead())
        {
            GameController.Instance.currentState = State.Death;
        }
    }

    void IsPlayerDead()
    {
        if (health <= 0)
        {
            Camera.main.transform.SetParent(null);
            gameObject.SetActive(false);
        }
    }

    bool AreAllPlayersDead()
    {
        foreach (GameObject playerObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (playerObj.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }

    public void IncreaseAttack(int multiplier)
    {
        attack *= multiplier;
    }
}
