using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skill", order = 1)]
[System.Serializable]
public class Skill : ScriptableObject
{

    public enum SkillType
    {
        Equip,
        Boost,
        Ability
    }

    public enum Ability
    {
        PassThrough,
        Bounce,
        ShootThroughEnemy,
        MegaArmor,
    }

    public enum TargetStat
    {
        None,
        HP,
        Attack,
        Shield,
        Speed,
        Dung,
        Defense,
    }

    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public SkillType skillType;
    public float statIncrease;
    public TargetStat targetStat;
    public Ability ability;


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


    public void ApplyEffect(Player player)
    {
        if (targetStat == TargetStat.Dung)
        {
            player.GetComponent<PlayerMovement>().dungAccumulationRate *= 2f;
        }
    }

    public void EnableAbility(PlayerAbilities playerAbilities)
    {
        if (ability == Ability.PassThrough)
        {
            playerAbilities.projectilePassThroughEnabled = true;
        }

        if (ability == Ability.Bounce)
        {
            playerAbilities.projectileBounceEnabled = true;
        }

        if (ability == Ability.ShootThroughEnemy)
        {
            playerAbilities.ShootThroughEnemiesEnabled();
        }

        if (ability == Ability.MegaArmor)
        {
            playerAbilities.MegaArmorEnabled();
        }
    }
}
