using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{

    public bool rapidShotEnabled = false;
    public bool burstShotEnabled = false;
    public bool attackUpEnabled = false;
    public bool healthUpEnabled = false;

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

    public void BurstShotEnabled()
    {
        burstShotEnabled = true;
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
}
