using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public class NestController : MonoBehaviour
{
    [Header("Stat Block Manager")]
    [SerializeField] GameObject statPanelObj;
    [SerializeField] Transform statGrid;

    [Header("Stat Point")]
    [SerializeField] Sprite statBlockSprite;

    [Header("Stat Blocks")]
    [SerializeField] List<StatBlock> statBlocks;
    Transform healthStats;
    Transform shieldStats;
    Transform attackStats;
    Transform defenseStats;
    Transform speedStats;
    Transform dungStats;

    [Header("Passive Manager")]
    [SerializeField] GameObject passiveObj;
    [SerializeField] Transform passiveHolder;
    [SerializeField] List<Passive> passives;

    [Header("Menu Buttons")]
    private Vector2 navigateMovement;
    [SerializeField] Button startButton;
    [SerializeField] Button backButton;

    [SerializeField] List<Button> passiveButtons;
    [SerializeField] List<Button> statUpgradeButtons;

    List<Button> buttons;
    [SerializeField] Color highlightedColor;
    int currentItemSelected = -1;


    [Header("Gem UI")]
    [SerializeField] Text gemText;
    public GameObject gemErrorPanel;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var statBlock in statBlocks)
        {
            SetUpStatPanel(statBlock.statType, statBlock);
        }

        foreach (var passive in passives)
        {
            SetUpPassivePanel();
        }

        SetupButtonList();
        UpdateGemText();
    }

    public void UpdateGemText()
    {
        gemText.text = PlayerBaseStatManager.instance.gems.ToString();
    }

    public void SetupButtonList()
    {
        buttons = passiveButtons.Concat(statUpgradeButtons).ToList();
        buttons.Add(startButton);
        buttons.Add(backButton);

        startButton.onClick.AddListener(() =>
        {
            HandleStartClick();
        });

        backButton.onClick.AddListener(() =>
        {
            HandleBackClick(backButton);
        });

    }

    public void SetUpStatPanel(StatType statType, StatBlock statBlock)
    {
        GameObject newStatBlock = Instantiate(statPanelObj, statGrid.position, statGrid.rotation);
        StatPanel newStatPanel = newStatBlock.GetComponent<StatPanel>();
        newStatBlock.transform.SetParent(statGrid, false);
        newStatPanel.UpdateCostText(statBlock.cost.ToString());
        newStatPanel.SetImage(statBlock.sprite);
        newStatPanel.UpdateSprite(statBlock.statBlockSprite);
        statUpgradeButtons.Add(newStatPanel.button);

        for (float i = 0; i < GetStatToCheck(statType); i += statBlock.increaseValue)
        {
            GameObject NewObj = new GameObject();
            Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
            NewImage.sprite = statBlockSprite;
            NewObj.transform.SetParent(newStatPanel.statHolder, false);
        }

        newStatPanel.button.onClick.AddListener(() =>
    {
        HandleStatIncrease(statType, statBlock.increaseValue, statBlock.cost, newStatPanel, newStatBlock);
    });
    }

    public void SetUpPassivePanel()
    {
        GameObject newPassive = Instantiate(passiveObj, passiveHolder.position, passiveHolder.rotation);
        PassivePanel newPassivePanel = newPassive.GetComponent<PassivePanel>();
        newPassive.transform.SetParent(passiveHolder, false);
        passiveButtons.Add(newPassivePanel.button);
    }

    public void HandleStartClick(Button button = null)
    {
        SceneManager.LoadScene(1);
    }


    public void HandleBackClick(Button button = null)
    {
        SceneManager.LoadScene(0);
    }

    bool CheckIfStatCanBeIncreased(StatType statType, float increaseValue, int cost)
    {
        int gemsAfterPurchase = PlayerBaseStatManager.instance.gems - cost;

        float statToCheck = GetStatToCheck(statType);

        if (statToCheck >= increaseValue * 50 || gemsAfterPurchase < 0)
        {
            StartCoroutine(ShowNotEnoughGemError());
            return false;
        }

        return true;
    }

    public void HandleStatIncrease(StatType statType, float increaseValue, int cost, StatPanel statPanel, GameObject newStatBlock)
    {
        if (CheckIfStatCanBeIncreased(statType, increaseValue, cost))
        {
            SoundManager.Instance.PlaySelectSound();
            PlayerBaseStatManager.instance.gems -= cost;

            IncreaseStat(statType, increaseValue);
            statPanel.AddStatPoint();
            UpdateGemText();
            SavingSystem.i.Save("saveSlot2");
        }
    }

    public float GetStatToCheck(StatType statType)
    {
        float statToCheck = 0;

        if (statType == StatType.Health)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusMaxHP;

        }

        if (statType == StatType.Shield)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusMaxShield;
        }

        if (statType == StatType.Attack)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusAttackPower;
        }

        if (statType == StatType.Defense)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusDefense;
        }

        if (statType == StatType.Speed)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusMoveSpeed;

        }

        if (statType == StatType.Dung)
        {
            statToCheck = PlayerBaseStatManager.instance.bonusMaxDung;
        }

        return statToCheck;
    }

    public void IncreaseStat(StatType statType, float increaseValue)
    {


        if (statType == StatType.Health)
        {

            PlayerBaseStatManager.instance.bonusMaxHP += (int)increaseValue;
        }

        if (statType == StatType.Shield)
        {

            PlayerBaseStatManager.instance.bonusMaxShield += (int)increaseValue;
        }

        if (statType == StatType.Attack)
        {

            PlayerBaseStatManager.instance.bonusAttackPower += (int)increaseValue;
        }

        if (statType == StatType.Defense)
        {

            PlayerBaseStatManager.instance.bonusDefense += (int)increaseValue;
        }

        if (statType == StatType.Speed)
        {

            PlayerBaseStatManager.instance.bonusMoveSpeed += increaseValue;
        }

        if (statType == StatType.Dung)
        {

            PlayerBaseStatManager.instance.bonusMaxDung += (int)increaseValue;
        }

    }

    public IEnumerator ShowNotEnoughGemError()
    {

        SoundManager.Instance.PlayErrorSound();
        gemErrorPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        gemErrorPanel.SetActive(false);

    }

    public void HandleUnlockPassive(Button button = null)
    {
        // subtract cost
        // Set player passive ability
        // Set stat passive gameobject to true
    }

    public void OnCancel(InputValue value)
    {
        SceneManager.LoadScene(0);
    }

    public void OnStart(InputValue value)
    {
        SceneManager.LoadScene(1);
    }

    public void OnNavigateUI(InputValue value)
    {
        navigateMovement = value.Get<Vector2>();

        SetAsUnselected();

        if (currentItemSelected == -1)
        {
            currentItemSelected = 0;
            SoundManager.Instance.PlaySwitchSound();
            return;
        }

        // On top row
        if (currentItemSelected <= passiveButtons.Count - 1)
        {
            HandlePassivePanelNavigation();
        }
        else if (currentItemSelected >= passiveButtons.Count)
        {

            HandleStatPanelNavigation();
        }

        HandleMenuSelectionOutOfBounds();

        SetAsSelected();
    }

    void HandleStatPanelNavigation()
    {
        if (navigateMovement.x > 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected++;
        }
        else if (navigateMovement.x < 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected--;
        }

        // Move to next section
        if (navigateMovement.y > 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected -= 2;
        }
        else if (navigateMovement.y < 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected += 2;
        }
    }

    void HandlePassivePanelNavigation()
    {
        if (navigateMovement.x > 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected++;
        }
        else if (navigateMovement.x < 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected--;
        }

        // Move to next section
        if (navigateMovement.y > 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected = buttons.Count - 1;
        }
        else if (navigateMovement.y < 0f)
        {
            SoundManager.Instance.PlaySwitchSound();
            currentItemSelected = passiveButtons.Count;
        }
    }

    public void OnInteract()
    {
        if (currentItemSelected == -1) { return; }
        buttons[currentItemSelected].onClick.Invoke();
    }

    void HandleMenuSelectionOutOfBounds()
    {
        if (currentItemSelected < 0)
        {
            currentItemSelected = buttons.Count - 1;
        }

        if (currentItemSelected > buttons.Count - 1)
        {
            currentItemSelected = 0;
        }
    }

    void SetAsSelected()
    {
        buttons[currentItemSelected].GetComponent<Image>().color = highlightedColor;
    }

    void SetAsUnselected()
    {
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }
}

