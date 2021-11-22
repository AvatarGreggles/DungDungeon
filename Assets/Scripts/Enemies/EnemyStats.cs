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

    private void Start()
    {
        maxHP = maxHP * LevelManager.Instance.floor;
        attackPower = attackPower * LevelManager.Instance.floor;
        moveSpeed = moveSpeed * LevelManager.Instance.floor;
        attackSpeed = attackSpeed * LevelManager.Instance.floor;
        defense = defense * LevelManager.Instance.floor;
        currencyDrop = currencyDrop * LevelManager.Instance.floor;
        expYield = expYield * LevelManager.Instance.floor;
    }
}