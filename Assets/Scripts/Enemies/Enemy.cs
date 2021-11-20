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

    private void Awake()
    {
        health = enemyStats.maxHP;
        collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        healthBar.SetActive(false);
        healthBarBackground.SetActive(false);
    }

    void OnEnable()
    {
        health = enemyStats.maxHP;
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        if (gameObject.activeSelf)
        {
            bool isPlayer = gameObject.CompareTag("Player");
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation(collider, isPlayer, gameObject));
            healthBar.transform.localScale = new Vector3(healthBar.transform.localScale.x * (health / enemyStats.maxHP), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
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
