using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();
        UpdateCritRatioText();

        foreach (Skill skill in skills)
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
}
