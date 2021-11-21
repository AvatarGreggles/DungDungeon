using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NewSkillScreen : MonoBehaviour
{

    [SerializeField] GameObject newSkillObject;
    public List<Skill> skills;

    [SerializeField] Transform skillList;


    [SerializeField] Text currencyText;

    [SerializeField] Text healthStatText;
    [SerializeField] Text shieldStatText;
    [SerializeField] Text attackStatText;
    [SerializeField] Text speedStatText;

    [SerializeField] Text critRatioStatText;

    [SerializeField] int skillOfferCount = 3;

    private Vector2 navigateMovement;

    int currentSkillSelected = -1;

    List<Skill> randomSkillItems;

    PlayerInput playerInput;


    Player player;

    // public void CloseSkillScreen()
    // {
    //     gameObject.SetActive(false);
    //     GameController.Instance.currentState = State.Active;
    // }


    public void UpdateCurrency()
    {
        currencyText.text = GameController.Instance.totalCurrency.ToString();
    }

    public void UpdateSpeedText()
    {
        speedStatText.text = player.attackSpeedBonus.ToString();
    }

    public void UpdateHPText()
    {
        healthStatText.text = player.health.ToString() + " / " + player.maxHealth.ToString();
    }

    public void UpdateShieldText()
    {
        shieldStatText.text = player.shield.ToString() + " / " + player.maxShield.ToString();
    }

    public void UpdateAttackText()
    {
        attackStatText.text = player.attack.ToString();
    }

    public void UpdateCritRatioText()
    {
        critRatioStatText.text = player.criticalHitRatio.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        playerInput = player.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("LevelUpMenu");

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();
        UpdateCritRatioText();

        randomSkillItems = HelperMethods.GetRandomItemsFromList<Skill>(skills, skillOfferCount);

        foreach (Skill skill in randomSkillItems)
        {
            GameObject newSkill = Instantiate(newSkillObject, newSkillObject.transform.position, newSkillObject.transform.rotation);
            if (newSkill)
            {
                newSkill.GetComponent<LevelSkill>().skill = skill;
                newSkill.GetComponent<LevelSkill>().SetSkillNameText();
                newSkill.GetComponent<LevelSkill>().SetSkillDescriptionText();
                newSkill.GetComponent<LevelSkill>().SetSkillIcon();
                newSkill.transform.SetParent(skillList);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleNavigation(InputValue value)
    {

        navigateMovement = value.Get<Vector2>();


        // if (navigateMovement.x == 0f)
        // {
        //     return;
        // }

        LevelSkill[] currentSkills = skillList.GetComponentsInChildren<LevelSkill>();
        foreach (LevelSkill skill in currentSkills)
        {
            skill.UnsetSkillAsSelected();
        }

        if (navigateMovement.x < 0f)
        {
            currentSkillSelected--;
        }
        else if (navigateMovement.x > 0f)
        {
            currentSkillSelected++;
        }

        if (currentSkillSelected < 0)
        {
            currentSkillSelected = currentSkills.Length - 1;
        }

        if (currentSkillSelected > currentSkills.Length - 1)
        {
            currentSkillSelected = 0;
        }

        currentSkills[currentSkillSelected].SetSkillAsSelected();

    }

    public void HandleInteract()
    {
        Debug.Log(currentSkillSelected);
        if (currentSkillSelected != -1)
        {
            LevelSkill[] currentSkills = skillList.GetComponentsInChildren<LevelSkill>();
            currentSkills[currentSkillSelected].ChooseSkill();
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
