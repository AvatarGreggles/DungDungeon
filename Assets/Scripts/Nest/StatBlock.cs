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
