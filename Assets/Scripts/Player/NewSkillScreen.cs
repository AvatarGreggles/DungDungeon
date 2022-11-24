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


    // [SerializeField] Text currencyText;

    [SerializeField] Text healthStatText;
    [SerializeField] Text shieldStatText;
    [SerializeField] Text attackStatText;
    [SerializeField] Text defenseStatText;
    [SerializeField] Text dungStatText;
    [SerializeField] Text speedStatText;

    [SerializeField] Text critRatioStatText;

    [SerializeField] int skillOfferCount = 3;

    private Vector2 navigateMovement;

    int currentSkillSelected = -1;

    List<Skill> randomSkillItems;

    PlayerInput playerInput;

    public AudioClip switchItemSound;
    public AudioClip selectItemSound;
    public AudioClip levelupSound;
    AudioSource audioSource;


    Player player;
    PlayerStatManager playerStatManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void UpdateSpeedText()
    {
        speedStatText.text = playerStatManager.attackSpeedBonus.ToString();
    }

    public void UpdateHPText()
    {
        healthStatText.text = playerStatManager.health.ToString() + " / " + playerStatManager.maxHealth.ToString();
    }

    public void UpdateShieldText()
    {
        shieldStatText.text = playerStatManager.shield.ToString() + " / " + playerStatManager.maxShield.ToString();
    }

    public void UpdateAttackText()
    {
        attackStatText.text = playerStatManager.attack.ToString();
    }

    public void UpdateCritRatioText()
    {
        critRatioStatText.text = playerStatManager.criticalHitRatio.ToString();
    }

    public void UpdateDungText()
    {
        dungStatText.text = playerStatManager.maxDungSize.ToString();
    }

    public void UpdateDefenseText()
    {
        defenseStatText.text = playerStatManager.defense.ToString();
    }


    public void GenerateSkills()
    {
        player = FindObjectOfType<Player>();
        playerInput = player.GetComponent<PlayerInput>();
        playerStatManager = player.GetComponent<PlayerStatManager>();
        // UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateDefenseText();
        UpdateSpeedText();
        UpdateDungText();
        UpdateCritRatioText();

        playerInput.actions.Disable();
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
                newSkill.transform.localScale = new Vector3(1f, 1f, 1f);
            }

        }
    }

    public void ClearSkills()
    {
        LevelSkill[] currentSkills = skillList.GetComponentsInChildren<LevelSkill>();

        foreach (LevelSkill skill in currentSkills)
        {
            skill.gameObject.SetActive(false);
        }
        currentSkillSelected = -1;
    }

    private void OnEnable()
    {
        GameController.Instance.StopGameMusic();
        audioSource.PlayOneShot(levelupSound, 1f);
        ClearSkills();
        GenerateSkills();

    }

    private void OnDisable()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.PlayGameMusic();
        }
    }


    public void OnNavigateUI(InputValue value)
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
            audioSource.PlayOneShot(switchItemSound, 1f);
            currentSkillSelected--;
        }
        else if (navigateMovement.x > 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
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

    public void OnInteract()
    {
        if (currentSkillSelected != -1)
        {
            playerInput.actions.Enable();
            audioSource.PlayOneShot(selectItemSound, 1f);


            LevelSkill[] currentSkills = skillList.GetComponentsInChildren<LevelSkill>();
            currentSkills[currentSkillSelected].ChooseSkill();
            currentSkills[currentSkillSelected].UnsetSkillAsSelected();
            currentSkillSelected = -1;
        }
    }
}
