using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAbilities : MonoBehaviour
{

    public bool rapidShotEnabled = false;
    public bool burstShotEnabled = false;
    public bool attackUpEnabled = false;
    public bool healthUpEnabled = false;

    public bool projectilePassThroughEnabled = false;
    public bool projectileBounceEnabled = false;

    public bool shootThroughEnemiesEnabled = false;

    public bool hpRegeneration = false;

    public bool isConfidenceEnabled = false;

    public int confidenceStack = 0;

    public bool isMegaArmorEnabled = false;

    public bool isGoldRushEnabled = false;

    public int goldRushStack = 0;

    public bool isBloodsuckerEnabled = false;
    public int bloodSuckerStack = 0;


    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RapidShotEnabled()
    {
        rapidShotEnabled = true;
    }

    public void ProjectilePassThroughEnabled()
    {
        projectilePassThroughEnabled = true;
    }

    public void GoldRushEnabled()
    {
        goldRushStack += 1;
        isGoldRushEnabled = true;
    }

    public void ProjectileBounceEnabled()
    {
        projectileBounceEnabled = true;
    }

    public void BloodsuckerEnabled()
    {
        bloodSuckerStack += 1;
        isBloodsuckerEnabled = true;
    }

    public void ShootThroughEnemiesEnabled()
    {
        shootThroughEnemiesEnabled = true;
    }

    public void BurstShotEnabled()
    {
        burstShotEnabled = true;
    }

    public void HPRegenerationEnabled()
    {
        hpRegeneration = true;
    }

    public void AttackUpEnabled()
    {
        player.IncreaseAttack(2);
        attackUpEnabled = true;
    }

    public void HealthUpEnabled()
    {
        healthUpEnabled = true;
    }

    public void ConfidenceEnabled()
    {
        confidenceStack += 1;
        isConfidenceEnabled = true;
    }

    public void MegaArmorEnabled()
    {
        isMegaArmorEnabled = true;
    }
}
