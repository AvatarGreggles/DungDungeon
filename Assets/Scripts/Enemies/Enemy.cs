using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject healthBarBackground;
    public GameObject damageDisplayPivot;
    public EnemyStats enemyStats;

    [SerializeField] GameObject loot;

    Collider2D collider;

    Vector3 initialHealthBarSize;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        health = enemyStats.maxHP;
        healthBar.SetActive(false);
        healthBarBackground.SetActive(false);

        initialHealthBarSize = healthBar.transform.localScale;
    }

    void OnEnable()
    {
        health = enemyStats.maxHP;
    }


    int GetAttackPower()
    {
        return enemyStats.attackPower * LevelManager.Instance.floor;
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        if (gameObject.activeSelf)
        {
            bool isPlayer = gameObject.CompareTag("Player");
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation(gameObject));
            healthBar.transform.localScale = new Vector3(initialHealthBarSize.x * (health / enemyStats.maxHP), initialHealthBarSize.y, initialHealthBarSize.z);
        }

        IsEnemyDead();
    }

    private void IsEnemyDead()
    {
        if (health <= 0)
        {
            LevelManager.Instance.enemies.Remove(gameObject);
            GameController.Instance.AddCurrency(enemyStats.currencyDrop);
            DropLoot();
            GivePlayersExperience();
            gameObject.SetActive(false);
        }
    }

    public void GivePlayersExperience()
    {
        foreach (Player player in GameController.Instance.players)
        {
            player.UpdateTempExperienceHolder(enemyStats.expYield, enemyStats.currencyDrop);

        }
    }

    private void DropLoot()
    {

        Instantiate(loot, transform.position, Quaternion.identity);
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("EntryAnimationEnded"))
        {
            healthBar.SetActive(true);
            healthBarBackground.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.activeSelf)
            {
                other.gameObject.GetComponent<Player>().DealDamage(1);
            }
        }
    }
}
