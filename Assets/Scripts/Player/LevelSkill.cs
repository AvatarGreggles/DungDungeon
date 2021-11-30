using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LevelSkill : MonoBehaviour
{

    [SerializeField] Image skillCardSpriteRenderer;
    [SerializeField] Sprite selectedSkillCard;
    [SerializeField] Sprite defaultSkillCard;

    //Todo: common, rare, epic?

    [SerializeField] public Skill skill;
    [SerializeField] Text skillNameText;
    [SerializeField] Text skillDescriptionText;

    [SerializeField] Image skillIcon;

    Player player;

    NewSkillScreen skillScreen;

    PlayerInput playerInput;

    public bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        skillScreen = FindObjectOfType<NewSkillScreen>();
        player = FindObjectOfType<Player>();
        playerInput = player.GetComponent<PlayerInput>();

        skillCardSpriteRenderer.sprite = defaultSkillCard;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSkillAsSelected()
    {

        skillCardSpriteRenderer.sprite = selectedSkillCard;
        isSelected = true;
    }

    public void UnsetSkillAsSelected()
    {

        skillCardSpriteRenderer.sprite = defaultSkillCard;
        isSelected = false;
    }

    public void SetSkillNameText()
    {
        skillNameText.text = skill.name;
    }

    public void SetSkillDescriptionText()
    {
        skillDescriptionText.text = skill.skillDescription;
    }

    public void SetSkillIcon()
    {
        skillIcon.sprite = skill.skillIcon;
    }

    public void ChooseSkill()
    {
        if (skill.skillType == Skill.SkillType.Boost)
        {
            skill.IncreaseStat(player, skillScreen);
        }

        if (skill.skillType == Skill.SkillType.Equip)
        {
            skill.ApplyEffect(player);
        }

        if (skill.skillType == Skill.SkillType.Ability)
        {
            PlayerAbilities playerAbilities = player.GetComponent<PlayerAbilities>();
            skill.EnableAbility(playerAbilities);
        }

        player.AddSkill(skill);

        GameController.Instance.currentState = State.Active;
        DoorManager.Instance.MoveToNextLevel();
        skillScreen.gameObject.SetActive(false);
        playerInput.actions.Enable();
        // playerInput.SwitchCurrentActionMap("Player");
    }
}
