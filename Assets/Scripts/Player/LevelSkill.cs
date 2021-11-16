using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSkill : MonoBehaviour
{

    [SerializeField] public Skill skill;
    [SerializeField] Text skillNameText;
    [SerializeField] Text skillDescriptionText;

    [SerializeField] Image skillIcon;

    Player player;

    NewSkillScreen skillScreen;

    // Start is called before the first frame update
    void Start()
    {
        skillScreen = FindObjectOfType<NewSkillScreen>();
        player = FindObjectsOfType<Player>()[0];
    }

    // Update is called once per frame
    void Update()
    {

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
        skill.IncreaseStat(player, skillScreen);
        GameController.Instance.currentState = State.Active;
        skillScreen.gameObject.SetActive(false);
    }
}
