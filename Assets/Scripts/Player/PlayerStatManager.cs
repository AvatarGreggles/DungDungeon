using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    PlayerBaseStatManager playerBaseStats;
    PlayerAbilities playerAbilities;
    Player player;

    [Header("Player Stats")]
    public float health;
    [SerializeField] public float maxHealth = 3;

    public float attack = 1;
    public float defense = 1;

    public float shield;
    [SerializeField] public float maxShield = 3;
    public float criticalHitRatio = 6.25f;
    public float attackSpeedBonus;

    public float dungAccumulated = 0f;
    public float prevDungAccumulated = 0f;

    public float maxDungSize = 4f;

    public int levelReached = 0;
    public int enemiesKilled = 0;
    public int moneyEarned = 0;

    [Header("Player HP Regeneration Details")]
    public int hpRegenDelay = 10; // Second count
    protected float hpRegenTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerAbilities = GetComponent<PlayerAbilities>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAbilities.hpRegeneration)
        {
            HandleHPRegeneration();
        }
    }

    void SetPlayerStats()
    {
        playerBaseStats = FindObjectOfType<PlayerBaseStatManager>();
        maxHealth = maxHealth + playerBaseStats.bonusMaxHP;
        health = maxHealth;
        maxShield = maxShield + playerBaseStats.bonusMaxShield;
        shield = maxShield;
        attack = attack + playerBaseStats.bonusAttackPower;
        defense = defense + playerBaseStats.bonusDefense;
        maxDungSize = maxDungSize + playerBaseStats.bonusMaxDung;
    }

    public void HandleHPRegeneration()
    {
        hpRegenTimer += Time.deltaTime;

        if (hpRegenTimer >= hpRegenDelay)
        {
            hpRegenTimer = 0f;
            if (health < maxHealth)
            {

                float amountToHeal = (maxHealth / 100) * 10;
                RestoreHealth(amountToHeal);
                player.UpdateHealthBar();
            }

            if (health == maxHealth)
            {
                player.UpdateHealthBar();
            }

        }
    }


    public void ResetHealth()
    {
        float amountToRestore = maxHealth - health;

        RestoreHealth(amountToRestore, true);
    }

    public void IncreaseAttack(int statIncrease)
    {
        attack += statIncrease;
    }

    public void RestoreHealth(float statIncrease, bool isReset = false)
    {
        if (health == maxHealth) { return; }
        health += statIncrease;
        // healthBar.GetComponent<SpriteRenderer>().color = Color.green;
        if (!isReset)
        {
            player.ShowHealthGain(statIncrease);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        player.UpdateHealthBar();
    }

}
