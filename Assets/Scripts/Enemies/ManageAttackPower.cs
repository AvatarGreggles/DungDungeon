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

        Debug.Log("attack power is" + attackPower);


        //TODO make getter functions that get the calculated stats
        if (GetComponentInParent<UbhShotCtrl>() != null)
        {
            GetComponentInParent<UbhShotCtrl>().m_shotList[0].m_shotObj.m_bulletPrefab.GetComponent<Projectile>().SetPower(attackPower);
            GetComponentInParent<UbhShotCtrl>().m_shotList[0].m_shotObj.m_bulletSpeed = attackSpeed * GetComponentInParent<Enemy>().enemyStats.level;
        }

    }

}
