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
        float multiplier = 1;

        if (statRange == StatRange.S)
        {
            multiplier = 1;
        }

        if (statRange == StatRange.M)
        {
            multiplier = 1.5f;
        }

        if (statRange == StatRange.L)
        {
            multiplier = 2;
        }

        if (statRange == StatRange.XL)
        {
            multiplier = 2.5f;
        }

        if (statRange == StatRange.XXL)
        {
            multiplier = 3;
        }

        if (statRange == StatRange.XXX)
        {
            multiplier = 3.5f;
        }

        if (statRange == StatRange.Nightmare)
        {
            multiplier = 4;
        }


        maxHP = baseMaxHP;
        attackPower = baseAttackPower;
        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        defense = baseDefense * level;
        currencyDrop = baseCurrencyDrop;
        expYield = baseExpYield;

        maxHP = (int)(maxHP * multiplier);
        attackPower = (int)(attackPower * multiplier);
        moveSpeed = (int)(moveSpeed * multiplier);
        attackSpeed = (int)(attackSpeed * multiplier);
        defense = (int)(defense * multiplier);
        currencyDrop = (int)(currencyDrop * multiplier);
        expYield = (int)(expYield * multiplier);
    }

}