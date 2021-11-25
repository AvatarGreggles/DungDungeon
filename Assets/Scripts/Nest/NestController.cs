using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NestController : MonoBehaviour
{

    [SerializeField] GameObject statBlockSprite;

    [SerializeField] Transform healthStats;
    [SerializeField] Transform shieldStats;
    [SerializeField] Transform attackStats;
    [SerializeField] Transform defenseStats;
    [SerializeField] Transform speedStats;
    [SerializeField] Transform dungStats;
    [SerializeField] Button startButton;
    [SerializeField] Button backButton;

    [SerializeField] Button increaseBonusHealth;
    [SerializeField] Button increaseBonusShield;
    [SerializeField] Button increaseBonusAttackPower;
    [SerializeField] Button increaseBonusDefense;
    [SerializeField] Button increaseBonusMoveSpeed;
    [SerializeField] Button increaseMaxDung;
    [SerializeField] Button unlockPassiveButton;

    // Start is called before the first frame update
    void Start()
    {
        GenerateStats();

        startButton.onClick.AddListener(() =>
    {
        HandleStartClick();
    });

        backButton.onClick.AddListener(() =>
       {
           HandleBackClick(backButton);
       });

        increaseBonusHealth.onClick.AddListener(() =>
        {
            HandleIncreaseBonusHealth(increaseBonusHealth);
        });


        increaseBonusShield.onClick.AddListener(() =>
       {
           HandleIncreaseBonusShield(increaseBonusShield);
       });

        increaseBonusAttackPower.onClick.AddListener(() =>
        {
            HandleIncreaseBonusAttack(increaseBonusAttackPower);
        });

        increaseBonusDefense.onClick.AddListener(() =>
       {
           HandleIncreaseBonusDefense(increaseBonusDefense);
       });

        increaseBonusMoveSpeed.onClick.AddListener(() =>
        {
            HandleIncreaseBonusMoveSpeed(increaseBonusMoveSpeed);
        });

        increaseMaxDung.onClick.AddListener(() =>
       {
           HandleIncreaseMaxDung(increaseMaxDung);
       });

        unlockPassiveButton.onClick.AddListener(() =>
        {
            HandleUnlockPassive(unlockPassiveButton);
        });
    }

    public void GenerateStats()
    {
        for (var i = 0; i < PlayerBaseStatManager.instance.bonusMaxHP; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, healthStats.position, healthStats.rotation);
            statPoint.transform.SetParent(healthStats);
        }

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusMaxHP; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, shieldStats.position, shieldStats.rotation);
            statPoint.transform.SetParent(shieldStats);
        }

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusAttackPower; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, attackStats.position, attackStats.rotation);
            statPoint.transform.SetParent(attackStats);
        }

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusDefense; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, defenseStats.position, defenseStats.rotation);
            statPoint.transform.SetParent(defenseStats);
        }

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusMoveSpeed; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, speedStats.position, speedStats.rotation);
            statPoint.transform.SetParent(speedStats);
        }

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusMaxDung; i++)
        {
            GameObject statPoint = Instantiate(statBlockSprite, dungStats.position, dungStats.rotation);
            statPoint.transform.SetParent(dungStats);
        }
    }

    public void HandleStartClick(Button button = null)
    {
        SceneManager.LoadScene(1);
    }


    public void HandleBackClick(Button button = null)
    {
        SceneManager.LoadScene(0);
    }

    public void HandleIncreaseBonusHealth(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMaxHP += 1;
        GameObject statPoint = Instantiate(statBlockSprite, healthStats.position, healthStats.rotation);
        statPoint.transform.SetParent(healthStats);
        // subtract cost
        // Increase player health by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusAttack(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusAttackPower += 1;
        GameObject statPoint = Instantiate(statBlockSprite, attackStats.position, attackStats.rotation);
        statPoint.transform.SetParent(attackStats);
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }


    public void HandleIncreaseBonusShield(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMaxShield += 1;
        GameObject statPoint = Instantiate(statBlockSprite, shieldStats.position, shieldStats.rotation);
        statPoint.transform.SetParent(shieldStats);
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusDefense(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusDefense += 1;
        GameObject statPoint = Instantiate(statBlockSprite, defenseStats.position, defenseStats.rotation);
        statPoint.transform.SetParent(defenseStats);
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusMoveSpeed(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMoveSpeed += 1;
        GameObject statPoint = Instantiate(statBlockSprite, speedStats.position, speedStats.rotation);
        statPoint.transform.SetParent(speedStats);
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }


    public void HandleIncreaseMaxDung(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMaxDung += 1;
        GameObject statPoint = Instantiate(statBlockSprite, dungStats.position, dungStats.rotation);
        statPoint.transform.SetParent(dungStats);
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }

    public void HandleUnlockPassive(Button button = null)
    {
        // subtract cost
        // Set player passive ability
        // Set stat passive gameobject to true
    }
}

