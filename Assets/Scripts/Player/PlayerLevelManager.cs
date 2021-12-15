using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    PlayerStatManager playerStatManager;

    [Header("Player Level & Experience")]

    public int level = 1;
    [SerializeField] int maxLevel = 100;

    [SerializeField] float baseXP;
    [SerializeField] TMPro.TMP_Text playerLevelTextP1;
    [SerializeField] public GameObject expBar;
    float experience;
    int previousLevel;

    float temporaryExperienceHolder;
    float experienceToNextLevel;
    int[] toLevelUp = new int[1];
    public bool willLevelUp = false;

    Vector3 initialEXPBarSize;

    private void Awake()
    {
        playerStatManager = GetComponent<PlayerStatManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        previousLevel = level;
        LevelXPSetUp();
        initialEXPBarSize = expBar.transform.localScale;
        UpdateExperienceBar();
        SetLevelText();
    }

    public void GainLevel()
    {
        level += 1;
        playerStatManager.UpdateMaxLevelReached();
        SetLevelText();
        playerStatManager.ResetHealth();
    }


    void SetLevelText()
    {
        playerLevelTextP1.text = level.ToString();
    }

    public void HandleExperienceGain()
    {
        StartCoroutine(FillExperienceBar(temporaryExperienceHolder));
    }

    void LevelXPSetUp()
    {
        toLevelUp = new int[maxLevel];
        for (int i = 1; i < toLevelUp.Length; i++)
        {
            toLevelUp[i] = (int)(Mathf.Floor(baseXP * (Mathf.Pow(i, 1.2f))));
        }
    }

    public void ResetLevelUp()
    {
        previousLevel = level;
        willLevelUp = false;
    }

    public void UpdateTemporaryEXP(float value)
    {
        temporaryExperienceHolder += value;
    }

    void CheckIfPlayerWillLevelUp(float experienceToAdd)
    {
        if (experience + experienceToAdd >= toLevelUp[level])
        {
            willLevelUp = true;
        }
    }

    void UpdateExperienceBar()
    {
        expBar.transform.localScale = new Vector3(initialEXPBarSize.x * (experience / toLevelUp[level]), initialEXPBarSize.y, initialEXPBarSize.z);

    }

    void HandleExpeienceOverflow()
    {
        experience = toLevelUp[level - 1] - experience;
        if (experience < 0)
        {
            experience *= 1;
        }
    }

    //should count up until it hits the experience amount to add.
    public IEnumerator FillExperienceBar(float experienceToAdd)
    {
        if (level < toLevelUp.Length)
        {
            temporaryExperienceHolder = 0;

            CheckIfPlayerWillLevelUp(experienceToAdd);

            // Slowly add experience
            for (int i = 0; i < experienceToAdd; i++)
            {
                if (level < toLevelUp.Length)
                {
                    // Increase experience and update exp bar
                    experience++;
                    UpdateExperienceBar();

                    // If the experience reaches the limit
                    if (experience >= toLevelUp[level])
                    {
                        GainLevel();

                        // If can not max level
                        if (level < toLevelUp.Length)
                        {
                            // Also handle exp overflow
                            HandleExpeienceOverflow();
                        }
                    }
                }
                yield return new WaitForSeconds(.001f);
            }
        }
    }
}
