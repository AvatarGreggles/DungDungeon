using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StatBlock", order = 1)]
public class StatBlock : ScriptableObject
{
    public int cost;
    public float increaseValue;
    public Action callback;

    public Sprite sprite;

    public StatType statType;

    public Sprite statBlockSprite;

    public void Refund()
    {
        if (statType == StatType.Health)
        {
            Debug.Log("health");
            float refund = (PlayerBaseStatManager.instance.bonusMaxHP / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusMaxHP = 0;

        }

        if (statType == StatType.Shield)
        {
            Debug.Log("shield");
            float refund = (PlayerBaseStatManager.instance.bonusMaxShield / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusMaxShield = 0;
        }

        if (statType == StatType.Attack)
        {
            float refund = (PlayerBaseStatManager.instance.bonusAttackPower / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusAttackPower = 0;
        }

        if (statType == StatType.Defense)
        {
            float refund = (PlayerBaseStatManager.instance.bonusDefense / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusDefense = 0;
        }

        if (statType == StatType.Speed)
        {
            float refund = (PlayerBaseStatManager.instance.bonusMoveSpeed / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusMoveSpeed = 0;

        }

        if (statType == StatType.Dung)
        {
            float refund = (PlayerBaseStatManager.instance.bonusMaxDung / increaseValue) * cost;
            PlayerBaseStatManager.instance.gems += (int)refund;
            PlayerBaseStatManager.instance.bonusMaxDung = 0;
        }
    }

}

public enum StatType
{
    Health,
    Shield,
    Attack,
    Defense,
    Speed,
    Dung
}
