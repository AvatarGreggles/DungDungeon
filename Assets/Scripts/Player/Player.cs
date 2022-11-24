using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Player : MonoBehaviour
{

    public PlayerAbilities playerAbilities;
    PlayerStatManager playerStatManager;
    PlayerLevelManager playerLevelManager;

    [SerializeField] int minimumDamageDeal = 50;

    [Header("Player Inventory")]
    [SerializeField] public List<Item> itemInventory;
    [SerializeField] public List<Skill> skillInventory;

    [Header("Player UI Manager")]
    [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject shieldBar;


    Vector3 initialHealthBarSize;
    Vector3 initialShieldBarSize;

    float initialDungBarSize;

    [Header("Player Display Pivots")]
    [SerializeField] GameObject criticalDamageSprite;
    [SerializeField] GameObject damageSprite;
    [SerializeField] GameObject healthDisplay;
    [SerializeField] GameObject damageDisplayPivot;
    [SerializeField] GameObject p1Tag;
    [SerializeField] GameObject p2Tag;

    [Header("Player Sound Effects")]
    [SerializeField] AudioClip hurtSound;
    AudioSource audioSource;


    [Header("Player Attack Details")]
    public SpriteRenderer dungSprite;
    public bool isShooting = false;

    [Header("Player Invincibility Framerate")]
    public float invincibilityFrameTime = 4f;
    float invincibilityTimer = 0;
    public bool isInvincible = false;

    LineRenderer lineRenderer;

    public List<GameObject> targettableEnemies;

    public int currentTargetIndex = 0;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerStatManager = GetComponent<PlayerStatManager>();
        playerLevelManager = GetComponent<PlayerLevelManager>();

        lineRenderer = GetComponent<LineRenderer>();

        // GameController.Instance.LoadData();

    }

    void Start()
    {
        SetUpPlayer();
    }

    private void Update()
    {
        HandleInvincibilityReset();
        UpdateDungSize();
    }

    void HandleInvincibilityReset()
    {
        if (isInvincible == true)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityFrameTime)
            {
                isInvincible = false;
            }
        }
        else
        {
            invincibilityTimer = 0;
        }
    }

    void UpdateDungSize()
    {
        if (playerStatManager.dungAccumulated != playerStatManager.prevDungAccumulated && playerStatManager.dungAccumulated > 0 && playerStatManager.dungAccumulated < playerStatManager.maxDungSize && !isShooting)
        {
            SetSpriteSize();
            playerStatManager.prevDungAccumulated = playerStatManager.dungAccumulated;
        }
    }

    void SetUpPlayer()
    {

        SetInitialPlayerUI();
        SetPlayerTag();

        GameController.Instance.dungBarP1.fillAmount = 0;
        dungSprite.enabled = false;
    }

    void SetInitialPlayerUI()
    {
        initialHealthBarSize = healthBar.transform.localScale;
        initialShieldBarSize = shieldBar.transform.localScale;

        healthBar.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void ShowHealthGain(float gainedHealth)
    {
        GameObject healthObject = Instantiate(healthDisplay, damageDisplayPivot.transform.position, damageDisplayPivot.transform.rotation);
        healthObject.transform.SetParent(transform);
        healthObject.GetComponent<DisplayHealth>().ShowHealth(gainedHealth);
        healthObject.transform.SetParent(null);
    }

    private void SetSpriteSize()
    {
        ShowSprite();
        dungSprite.transform.localScale += new Vector3(0.025f, 0.025f, 0f);
    }

    public void AccumulateDung(float dungAccumulationRate)
    {
        playerStatManager.dungAccumulated += dungAccumulationRate;
        UpdateDungUI();
    }

    void UpdateDungUI()
    {
        GameController.Instance.dungBarP1.fillAmount = playerStatManager.dungAccumulated / playerStatManager.maxDungSize;
        GameController.Instance.SetDungText(playerStatManager.dungAccumulated);
    }


    public void AddItem(Item item)
    {
        itemInventory.Add(item);
    }

    public void AddSkill(Skill skill)
    {
        skillInventory.Add(skill);
    }


    public void ResetShield()
    {
        playerStatManager.shield = playerStatManager.maxShield;
        shieldBar.transform.localScale = new Vector3(initialShieldBarSize.x * (playerStatManager.shield / playerStatManager.maxShield), initialShieldBarSize.y, initialShieldBarSize.z);

    }

    public void ResetInvincibility()
    {
        isInvincible = false;
    }

    public void HandlePlayerGains(float value, int currency)
    {
        playerStatManager.moneyEarned += currency;
        playerStatManager.enemiesKilled++;
        RunAbilityGains();
        playerLevelManager.UpdateTemporaryEXP(value);
    }

    // ===============================Dung Sprite=============================
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

    // ================================Tags=========================
    void SetPlayerTag()
    {
        p1Tag.SetActive(true);
    }


    // ======================Combat===================================
    int RunPlayerAbilities(int damage)
    {
        // Mega Armor: Doubles damage when health is less than 25%
        if (playerAbilities.isMegaArmorEnabled && playerStatManager.health < playerStatManager.maxHealth / 4)
        {
            float boostedDefense = playerStatManager.defense * 2;
            damage -= (int)boostedDefense;
        }
        else
        {
            damage -= (int)playerStatManager.defense;
        }

        return damage;
    }

    public void RunAbilityGains()
    {
        if (playerAbilities.isConfidenceEnabled)
        {
            int amountToIncrease = 1 * playerAbilities.confidenceStack;
            playerStatManager.IncreaseAttack(amountToIncrease);

        }

        if (playerAbilities.isBloodsuckerEnabled)
        {
            float amountToRestore = (playerStatManager.maxHealth / 100) * (5 * playerAbilities.bloodSuckerStack);
            playerStatManager.RestoreHealth(amountToRestore);
        }
    }

    void HandleDamageOutOfBounds(int damage)
    {
        damage = minimumDamageDeal;
        Debug.Log("Damage was below 0, so rounding up to minimum!");
    }

    void HandleDamageDisplay(bool isCriticalHit, int damage)
    {
        GameObject spriteToInstantiate = isCriticalHit ? criticalDamageSprite : damageSprite;
        GameObject damageObject = Instantiate(spriteToInstantiate, damageDisplayPivot.transform.position, damageDisplayPivot.transform.rotation);
        damageObject.transform.SetParent(transform);
        damageObject.GetComponent<DisplayDamage>().showDamage(damage);
        damageObject.transform.SetParent(null);
    }

    void HandleDamageDealing(int damage)
    {

        if (playerStatManager.shield > 0)
        {
            playerStatManager.shield = Mathf.Clamp(playerStatManager.shield - damage, 0, playerStatManager.maxShield);
            shieldBar.transform.localScale = new Vector3(initialShieldBarSize.x * (playerStatManager.shield / playerStatManager.maxShield), initialShieldBarSize.y, initialShieldBarSize.z);
        }
        else
        {
            shieldBar.transform.localScale = new Vector3(0f, initialShieldBarSize.y, initialShieldBarSize.z);
            playerStatManager.health = Mathf.Clamp(playerStatManager.health - damage, 0, playerStatManager.maxHealth);
            UpdateHealthBar();

        }
    }

    public void DealDamage(int initialDamage, bool isCriticalHit)
    {
        if (isInvincible) { return; }
        isInvincible = true;

        int damage = RunPlayerAbilities(initialDamage);
        if (damage < 0)
        {
            HandleDamageOutOfBounds(damage);
        }

        HandleDamageDisplay(isCriticalHit, damage);

        HandleDamageDealing(damage);


        if (gameObject != null)
        {
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation());
        }

        audioSource.PlayOneShot(hurtSound, 1f);

        IsPlayerDead();

        if (AreAllPlayersDead())
        {
            GameController.Instance.currentState = State.Death;
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3(initialHealthBarSize.x * (playerStatManager.health / playerStatManager.maxHealth), initialHealthBarSize.y, initialHealthBarSize.z);
        UpdateHPBarColor();
    }

    void UpdateHPBarColor()
    {
        if (playerStatManager.health < playerStatManager.maxHealth / 4)
        {
            healthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }

    }

    public GameObject FindClosestTarget(string targetTag)
    {
        //TODO copy the logic and use it t trigger a draw to player function for coins
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject closest = null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }

    // Death logic
    void IsPlayerDead()
    {
        if (playerStatManager.health <= 0)
        {
            Camera.main.transform.SetParent(null);
            gameObject.SetActive(false);
        }
    }

    // Game over logic
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

    public void OnSwitchTarget()
    {
        // if theres an enemy available
        if (targettableEnemies.Count > 0)
        {
            // If theres only one, make sure we set the current index to 0
            if (targettableEnemies.Count == 1)
            {
                currentTargetIndex = 0;
                SetEnemyAsTargetted();
                return;
            }

            if (currentTargetIndex > targettableEnemies.Count - 1)
            {
                currentTargetIndex = 0;
                SetEnemyAsTargetted();
                return;
            }

            // if (currentTargetIndex > targettableEnemies.Count)
            // {
            //     currentTargetIndex = 0;
            // }

            if (targettableEnemies.Count > 1)
            {
                targettableEnemies[currentTargetIndex].GetComponent<Enemy>().Untarget();
                currentTargetIndex++;
                if (currentTargetIndex > targettableEnemies.Count - 1)
                {
                    currentTargetIndex = 0;
                }
                SetEnemyAsTargetted();
            }
        }
        else
        {
            Debug.Log("nothing to target in range");
        }
    }

    public void SetEnemyAsTargetted()
    {
    //     if (currentTargetIndex > targettableEnemies.Count - 1)
    //     {
    //         currentTargetIndex = 0;
    //     }

    //     targettableEnemies[currentTargetIndex].GetComponent<Enemy>().SetAsTargetted();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            targettableEnemies.Add(other.gameObject);
            if (targettableEnemies.Count == 1)
            {
                SetEnemyAsTargetted();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            targettableEnemies.Remove(other.gameObject);
            other.gameObject.GetComponent<Enemy>().Untarget();
            if (targettableEnemies.Count == 0)
            {
                currentTargetIndex = 0;
            }
        }
    }
}
