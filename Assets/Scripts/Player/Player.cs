using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{

    public float health;

    public float experience;

    public float baseXP;
    public float temporaryExperienceHolder;
    public float experienceToNextLevel;
    public int level;

    [SerializeField] public float maxHealth = 3;

    public float attackSpeedBonus;

    public float shield;
    [SerializeField] public float maxShield = 3;
    [SerializeField] GameObject healthBar;
    public float attack = 1;
    public GameObject damageDisplayPivot;

    [SerializeField] GameObject p1Tag;
    [SerializeField] GameObject p2Tag;

    [SerializeField] PlayerInput playerInput;

    public AudioClip hurtSound;
    AudioSource audioSource;

    Vector3 initialHealthBarSize;

    public SpriteRenderer dungSprite;

    public float dungAccumulated = 0f;
    public float prevDungAccumulated = 0f;

    public float maxDungSize = 4f;

    public bool isShooting = false;

    public int[] toLevelUp = new int[1];

    [SerializeField] TMPro.TMP_Text playerLevelTextP1;
    // [SerializeField] Text playerLevelTextP2;

    [SerializeField] TMPro.TMP_Text playerEXPTextP1;
    [SerializeField] TMPro.TMP_Text playerTempEXPTextP1;




    private void Awake()
    {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        playerInput = GetComponent<PlayerInput>();
    }


    void Start()
    {
        SetPlayerTag();
        initialHealthBarSize = healthBar.transform.localScale;
        SetLevelText();
        SetEXPText();
        SetTempEXPText();
        LevelXPSetUp();

        dungSprite.enabled = false;
    }

    private void Update()
    {
        if (dungAccumulated != prevDungAccumulated && dungAccumulated > 0 && dungAccumulated < maxDungSize && !isShooting)
        {
            SetSpriteSize();
            prevDungAccumulated = dungAccumulated;
        }
    }

    private void SetSpriteSize()
    {
        ShowSprite();
        dungSprite.transform.localScale += new Vector3(0.025f, 0.025f, 0f);
    }

    public void GainLevel()
    {
        level += 1;
        SetLevelText();
        GameController.Instance.currentState = State.LevelUp;
    }

    void SetLevelText()
    {
        if (playerInput.playerIndex == 0)
        {
            playerLevelTextP1.text = level.ToString();
        }
    }

    void SetEXPText()
    {
        if (playerInput.playerIndex == 0)
        {
            playerEXPTextP1.text = experience.ToString();
        }
    }

    void SetTempEXPText()
    {
        if (playerInput.playerIndex == 0)
        {
            playerTempEXPTextP1.text = temporaryExperienceHolder.ToString();
        }
    }

    void LevelXPSetUp()
    {
        for (int i = 1; i < toLevelUp.Length; i++)
        {
            toLevelUp[i] = (int)(Mathf.Floor(baseXP * (Mathf.Pow(i, 1.2f))));
        }
    }

    // public void GainEXP(float value)
    // {
    //     experience += value;
    //     if (HasReachedNextLevel())
    //     {
    //         experience = toLevelUp[level] - experience;
    //         if (experience < 0)
    //         {
    //             experience *= 1;
    //         }
    //         Debug.Log(experience);
    //         temporaryExperienceHolder = 0;
    //         SetTempEXPText();
    //     }
    //     SetEXPText();
    // }

    //should count up until it hits the experience amount to add.
    public IEnumerator FillExperienceBar(float experienceToAdd)
    {

        temporaryExperienceHolder = 0;
        SetTempEXPText();
        // addingXp = true;
        //received from external sources. Add xp incrementally to move bar up slowly instead of chunks.
        for (int i = 0; i < experienceToAdd; i++)
        {
            experience++;
            SetEXPText();
            if (HasReachedNextLevel())
            {
                experience = toLevelUp[level - 1] - experience;
                if (experience < 0)
                {
                    experience *= 1;
                }
            }
            yield return new WaitForSeconds(.001f);
        }

        // addingXp = false;
    }

    public void MergeTempExperience()
    {
        StartCoroutine(FillExperienceBar(temporaryExperienceHolder));
        // GainEXP(temporaryExperienceHolder);
    }

    public void UpdateTempExperienceHolder(float value)
    {
        temporaryExperienceHolder += value;
        SetTempEXPText();
    }

    public bool HasReachedNextLevel()
    {
        if (experience >= toLevelUp[level])
        {
            GainLevel();
            return true;
        }
        else
        {
            return false;
        }
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

    public void DealDamage(int damage)
    {
        health -= damage;
        healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * (health / maxHealth), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        audioSource.PlayOneShot(hurtSound, 1f);

        IsPlayerDead();

        if (AreAllPlayersDead())
        {
            GameController.Instance.currentState = State.Paused;
        }
    }

    void IsPlayerDead()
    {
        if (health <= 0)
        {
            Camera.main.transform.SetParent(null);
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation());
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
