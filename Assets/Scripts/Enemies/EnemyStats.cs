using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public int level = 1;

    public enum StatRange
    {
        S,
        M,
        L,
        XL,
        XXL,
        XXX,
        Nightmare,
    }
    public int baseMaxHP = 1;
    public int baseAttackPower = 1;
    public int baseMoveSpeed = 1;
    public int baseAttackSpeed = 1;
    public int baseDefense = 1;
    public int baseCurrencyDrop = 1;
    public int baseExpYield = 1;

    public int maxHP = 1;
    public int attackPower = 1;
    public int moveSpeed = 1;
    public int attackSpeed = 0;
    public int defense = 0;
    public int currencyDrop = 1;
    public int expYield = 1;

    public StatRange statRange;

    void OnEnable()
    {
        int multiplier = 1;

        if (statRange == StatRange.S)
        {
            multiplier = 1;
        }

        if (statRange == StatRange.M)
        {
            multiplier = 2;
        }

        if (statRange == StatRange.L)
        {
            multiplier = 3;
        }

        if (statRange == StatRange.XL)
        {
            multiplier = 4;
        }

        if (statRange == StatRange.XXL)
        {
            multiplier = 5;
        }

        if (statRange == StatRange.XXX)
        {
            multiplier = 6;
        }

        if (statRange == StatRange.Nightmare)
        {
            multiplier = 7;
        }


        maxHP = baseMaxHP;
        attackPower = baseAttackPower;
        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        defense = baseDefense * level;
        currencyDrop = baseCurrencyDrop;
        expYield = baseExpYield;

        maxHP = maxHP * multiplier;
        attackPower = attackPower * multiplier;
        moveSpeed = moveSpeed * multiplier;
        attackSpeed = attackSpeed * multiplier;
        defense = defense * multiplier;
        currencyDrop = currencyDrop * multiplier;
        expYield = expYield * multiplier;
    }

}