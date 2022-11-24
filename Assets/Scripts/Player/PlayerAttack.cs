using System.Collections;
using System;
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
    }

    void Update(){
         bool hasEnoughDung = playerStatManager.dungAccumulated >= attackCost;

         if(hasEnoughDung){
              StartCoroutine(Shooting());

         }
    }


    void OnShoot(InputValue value)
    {
        // bool hasEnoughDung = playerStatManager.dungAccumulated >= attackCost;

        if (!player.isShooting)
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
                player.isShooting = true;

                GameObject projectileObj = Instantiate(m_Projectile, m_SpawnTransform.position, m_Projectile.transform.rotation);
                Projectile projectile = projectileObj.GetComponent<Projectile>();

                projectile.SetScale(player);

                RunProjectileAbilities(projectile);

                HandlePowerCalculation(projectile);

                playerStatManager.dungAccumulated = 0;
                GameController.Instance.SetDungText(playerStatManager.dungAccumulated);
                player.isShooting = false;

                yield break;

            }

            if (LevelManager.Instance.enemies.Count <= 0)
            {
                yield break;
            }
        }
    }

    void HandlePowerCalculation(Projectile projectile)
    {
        float bonusCritDamage = HandleCritBonusCalc(projectile);

        SetProjectilePower(projectile, playerStatManager.attack * bonusCritDamage);
    }

    float HandleCritBonusCalc(Projectile projectile)
    {
        float bonusCritDamage = 1f;
        if (UnityEngine.Random.value * 100f <= playerStatManager.criticalHitRatio)
        {

            projectile.isCriticalHit = true;
            bonusCritDamage = 2f;
        }

        return bonusCritDamage;
    }

    void SetProjectilePower(Projectile projectile, float power)
    {
        projectile.power = Convert.ToInt32(Mathf.Round((power))); // TODO: Modifier for dung collected player.dungAccumulated / 10

    }

    void RunProjectileAbilities(Projectile projectile)
    {
        if (playerAbilities.projectilePassThroughEnabled)
        {
            projectile.EnablePassThroughWall();
        }

        if (playerAbilities.projectileBounceEnabled)
        {
            projectile.EnableProjectileBounce();
        }
    }
}
