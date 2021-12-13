using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    public GameObject m_Projectile;    // this is a reference to your projectile prefab
    public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    public bool isShooting = false;

    [SerializeField] float attackDelay = 1f;

    [SerializeField] float attackCost = 5f;

    [SerializeField] PlayerAbilities playerAbilities;

    PlayerMovement playerMovement;

    public static PlayerAttack Instance { get; set; }

    public bool attackReleased = false;

    Player player;

    [SerializeField] PlayerInput playerInput;

    PlayerBaseStatManager playerBaseStatManager;

    PlayerStatManager playerStatManager;


    private void Awake()
    {
        Instance = this;

        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        playerStatManager = GetComponent<PlayerStatManager>();
    }

    private void Start()
    {

        playerBaseStatManager = FindObjectOfType<PlayerBaseStatManager>();
        // StartCoroutine(Shooting());
    }


    void OnShoot(InputValue value)
    {
        if (!player.isShooting && playerStatManager.dungAccumulated >= attackCost)
        {
            StartCoroutine(Shooting());
        }
    }

    public IEnumerator Shooting()
    {
        while (true && gameObject.activeInHierarchy == true)
        {
            // yield return new WaitForSeconds(attackDelay);
            if (m_Projectile != null && m_SpawnTransform != null)
            {
                if (playerStatManager.dungAccumulated >= attackCost)
                // if (attackReleased && player.dungAccumulated >= attackCost)
                // if (attackReleased)
                {
                    player.isShooting = true;

                    GameObject projectile = Instantiate(m_Projectile, m_SpawnTransform.position, m_Projectile.transform.rotation);
                    projectile.GetComponent<Projectile>().SetScale(player);
                    if (playerAbilities.projectilePassThroughEnabled)
                    {
                        projectile.GetComponent<Projectile>().EnablePassThroughWall();
                    }

                    if (playerAbilities.projectileBounceEnabled)
                    {
                        projectile.GetComponent<Projectile>().EnableProjectileBounce();
                    }



                    // COST BASED
                    // playerMovement.dungAccumulated -= attackCost;

                    float bonusCritDamage = 1f;
                    if (UnityEngine.Random.value * 100f <= playerStatManager.criticalHitRatio)
                    {

                        projectile.GetComponent<Projectile>().isCriticalHit = true;
                        bonusCritDamage = 2f;
                    }

                    projectile.GetComponent<Projectile>().power = Convert.ToInt32(Mathf.Round((playerStatManager.attack * bonusCritDamage))); // TODO: Modifier for dung collected player.dungAccumulated / 10
                    playerStatManager.dungAccumulated = 0;
                    GameController.Instance.SetDungText(playerStatManager.dungAccumulated, playerInput);
                    GameController.Instance.SetDungText(playerStatManager.dungAccumulated, playerInput);
                    attackReleased = false;
                    player.isShooting = false;
                    yield break;
                }

                if (!playerAbilities.burstShotEnabled)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    yield return new WaitForSeconds(0.025f);
                }


                // if (playerAbilities.rapidShotEnabled)
                // {
                //     if (playerMovement.dungAccumulated >= attackCost)
                //     {
                //         Instantiate(m_Projectile, m_SpawnTransform.position, m_Projectile.transform.rotation);
                //     }
                // }

                // if (playerAbilities.burstShotEnabled)
                // {
                //     yield return new WaitForSeconds(0.025f);
                // }

                if (LevelManager.Instance.enemies.Count <= 0)
                {
                    yield break;
                }
            }
            else
            {
                yield break;
            }

        }
    }
}
