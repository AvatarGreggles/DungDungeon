using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAttackPower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int attackPower = GetComponentInParent<Enemy>().enemyStats.attackPower;
        int attackSpeed = GetComponentInParent<Enemy>().enemyStats.attackSpeed;


        //TODO make getter functions that get the calculated stats
        GetComponent<UbhLinearLockOnShot>().m_bulletPrefab.GetComponent<Projectile>().SetPower(attackPower);
        GetComponent<UbhLinearLockOnShot>().m_bulletSpeed = attackSpeed * GetComponentInParent<Enemy>().enemyStats.level;

    }

}
