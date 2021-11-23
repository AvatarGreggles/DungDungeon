using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public int level = 1;
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

    void OnEnable()
    {
        maxHP = baseMaxHP;
        attackPower = baseAttackPower;
        moveSpeed = baseMoveSpeed;
        attackSpeed = baseAttackSpeed;
        defense = baseDefense * level;
        currencyDrop = baseCurrencyDrop;
        expYield = baseExpYield;

        maxHP = maxHP * level;
        attackPower = attackPower * level;
        moveSpeed = moveSpeed * level;
        attackSpeed = attackSpeed * level;
        defense = defense * level;
        currencyDrop = currencyDrop * level;
        expYield = expYield * level;
    }

}