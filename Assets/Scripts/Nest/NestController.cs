using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    [SerializeField] List<Button> buttons;
    int currentItemSelected = -1;

    public AudioClip switchItemSound;
    public AudioClip selectItemSound;
    AudioSource audioSource;

    private Vector2 navigateMovement;

    [SerializeField] Button increaseBonusHealth;
    [SerializeField] Button increaseBonusShield;
    [SerializeField] Button increaseBonusAttackPower;
    [SerializeField] Button increaseBonusDefense;
    [SerializeField] Button increaseBonusMoveSpeed;
    [SerializeField] Button increaseMaxDung;
    [SerializeField] Button unlockPassiveButton;

    private void Awake()
    {
        // SavingSystem.i.Load("saveSlot1");
        audioSource = GetComponent<AudioSource>();
    }

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

        for (var i = 0; i < PlayerBaseStatManager.instance.bonusMaxShield; i++)
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
        SavingSystem.i.Save("saveSlot3");
        // subtract cost
        // Increase player health by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusAttack(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusAttackPower += 1;
        GameObject statPoint = Instantiate(statBlockSprite, attackStats.position, attackStats.rotation);
        statPoint.transform.SetParent(attackStats);
        SavingSystem.i.Save("saveSlot3");
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }


    public void HandleIncreaseBonusShield(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMaxShield += 1;
        GameObject statPoint = Instantiate(statBlockSprite, shieldStats.position, shieldStats.rotation);
        statPoint.transform.SetParent(shieldStats);
        SavingSystem.i.Save("saveSlot3");
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusDefense(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusDefense += 1;
        GameObject statPoint = Instantiate(statBlockSprite, defenseStats.position, defenseStats.rotation);
        statPoint.transform.SetParent(defenseStats);
        SavingSystem.i.Save("saveSlot3");
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }

    public void HandleIncreaseBonusMoveSpeed(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMoveSpeed += 1;
        GameObject statPoint = Instantiate(statBlockSprite, speedStats.position, speedStats.rotation);
        statPoint.transform.SetParent(speedStats);
        SavingSystem.i.Save("saveSlot3");
        // subtract cost
        // Increase player attack by 1
        // Set stat increase gameobject to true
    }


    public void HandleIncreaseMaxDung(Button button = null)
    {
        PlayerBaseStatManager.instance.bonusMaxDung += 1;
        GameObject statPoint = Instantiate(statBlockSprite, dungStats.position, dungStats.rotation);
        statPoint.transform.SetParent(dungStats);
        SavingSystem.i.Save("saveSlot3");
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

    public void OnNavigateUI(InputValue value)
    {
        navigateMovement = value.Get<Vector2>();


        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }

        // On top row
        if (currentItemSelected <= 8)
        {
            if (currentItemSelected == -1)
            {
                currentItemSelected = 0;
                audioSource.PlayOneShot(switchItemSound, 1f);
                return;
            }

            if (navigateMovement.x > 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected++;
            }
            else if (navigateMovement.x < 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected--;
            }

            // Move to next section
            if (navigateMovement.y > 0f)
            {
                return;
            }
            else if (navigateMovement.y < 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected = 9;
            }
        }
        else if (currentItemSelected > 8)
        {

            if (navigateMovement.x > 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected++;
            }
            else if (navigateMovement.x < 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected--;
            }

            // Move to next section
            if (navigateMovement.y > 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected -= 2;
            }
            else if (navigateMovement.y < 0f)
            {
                audioSource.PlayOneShot(switchItemSound, 1f);
                currentItemSelected += 2;
            }
        }

        if (currentItemSelected < 0)
        {
            currentItemSelected = buttons.Count - 1;
        }

        if (currentItemSelected > buttons.Count - 1)
        {
            currentItemSelected = 0;
        }


        buttons[currentItemSelected].GetComponent<Image>().color = new Color(0.5f, 0.4f, 0.2f);
    }

    public void OnInteract()
    {
        if (currentItemSelected == -1) { return; }
        audioSource.PlayOneShot(selectItemSound, 1f);
        buttons[currentItemSelected].onClick.Invoke();
    }
}

