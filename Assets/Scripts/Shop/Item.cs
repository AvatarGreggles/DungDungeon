using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{

    public enum ItemType
    {
        Equip,
        Boost,
        Heal,
    }

    public enum TargetStat
    {
        None,
        HP,
        Attack,
        Shield,
        Speed,
        Dung,
        CritChance,
    }

    public string itemName;
    public string itemDescription;
    public int price;
    public Sprite itemIcon;
    public ItemType itemType;
    public float statIncrease;
    public TargetStat targetStat;

    public Ability ability;

    public enum Ability
    {
        None,
        HPRegeneration,
        ShootThroughEnemy
    }


    public void HealStat(Player player, Shop shop)
    {
        if (targetStat == TargetStat.HP)
        {
            player.health += statIncrease;
            if (player.health > player.maxHealth)
            {
                player.health = player.maxHealth;
            }
            shop.UpdateHPText();
            player.UpdateHealthBar();
        }
    }

    public void IncreaseStat(Player player, Shop shop)
    {

        if (targetStat == TargetStat.Attack)
        {
            player.attack *= statIncrease;
            shop.UpdateAttackText();
        }

        if (targetStat == TargetStat.HP)
        {
            player.maxHealth += statIncrease;
            shop.UpdateHPText();
        }

        if (targetStat == TargetStat.Shield)
        {
            player.maxShield += statIncrease;
            shop.UpdateShieldText();
        }

        if (targetStat == TargetStat.Speed)
        {
            player.attackSpeedBonus += statIncrease;
            shop.UpdateSpeedText();
        }

        if (targetStat == TargetStat.CritChance)
        {
            player.criticalHitRatio += statIncrease;
            shop.UpdateCritRatioText();
        }
    }

    public void EnableAbility(PlayerAbilities playerAbilities)
    {
        if (ability == Ability.HPRegeneration)
        {
            playerAbilities.HPRegenerationEnabled();
        }

        if (ability == Ability.HPRegeneration)
        {
            playerAbilities.HPRegenerationEnabled();
        }

        if (ability == Ability.ShootThroughEnemy)
        {
            playerAbilities.ShootThroughEnemiesEnabled();
        }
    }

}
