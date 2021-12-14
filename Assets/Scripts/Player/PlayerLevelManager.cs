using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    PlayerStatManager playerStatManager;

    [Header("Player Level & Experience")]

    public int level;
    public float experience;
    public int previousLevel;
    public float baseXP;
    public float temporaryExperienceHolder;
    public float experienceToNextLevel;
    public int[] toLevelUp = new int[1];

    public bool willLevelUp = false;

    [SerializeField] TMPro.TMP_Text playerLevelTextP1;

    [SerializeField] public GameObject expBar;

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
        expBar.transform.localScale = new Vector3(initialEXPBarSize.x * (experience / toLevelUp[level]), initialEXPBarSize.y, initialEXPBarSize.z);
        SetLevelText();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void MergeTempExperience()
    {
        StartCoroutine(FillExperienceBar(temporaryExperienceHolder));
        // GainEXP(temporaryExperienceHolder);
    }

    void LevelXPSetUp()
    {
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
}
