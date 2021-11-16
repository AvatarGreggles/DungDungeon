using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skill", order = 1)]
public class Skill : ScriptableObject
{

    public enum SkillType
    {
        Equip,
        Boost
    }

    public enum TargetStat
    {
        None,
        HP,
        Attack,
        Shield,
        Speed,
        Dung,
    }

    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public SkillType skillType;
    public float statIncrease;
    public TargetStat targetStat;


    public void IncreaseStat(Player player, NewSkillScreen skillScreen)
    {
        if (targetStat == TargetStat.Attack)
        {
            player.attack += statIncrease;
            skillScreen.UpdateAttackText();
        }

        if (targetStat == TargetStat.HP)
        {
            player.maxHealth += statIncrease;
            skillScreen.UpdateHPText();
        }

        if (targetStat == TargetStat.Shield)
        {
            player.maxShield += statIncrease;
            skillScreen.UpdateShieldText();
        }

        if (targetStat == TargetStat.Speed)
        {
            player.attackSpeedBonus += statIncrease;
            skillScreen.UpdateSpeedText();
        }
    }



}
