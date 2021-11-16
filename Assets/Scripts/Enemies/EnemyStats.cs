using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public int maxHP = 1;
    public int attackPower = 1;
    public int moveSpeed = 1;
    public int attackSpeed = 0;
    public int defense = 0;
    public int currencyDrop = 1;
    public int expYield = 1;
}