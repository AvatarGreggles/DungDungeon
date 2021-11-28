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

    [SerializeField] GameObject criticalDamageSprite;
    [SerializeField] GameObject damageSprite;

    Animator animator;

    UbhShotCtrl shotCtrl;

    public bool isDead = false;

    bool spawned = false;


    public AudioClip cry;
    AudioSource audioSource;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        shotCtrl = GetComponent<UbhShotCtrl>();
        audioSource = GetComponent<AudioSource>();

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

    public void DealDamage(int damage, bool isCriticalHit)
    {
        GameObject spriteToInstantiate = isCriticalHit ? criticalDamageSprite : damageSprite;
        // GameObject spriteToInstantiate = damageSprite;
        GameObject damageObject = Instantiate(spriteToInstantiate, damageDisplayPivot.transform.position, damageDisplayPivot.transform.rotation);
        damageObject.transform.SetParent(transform);
        damageObject.GetComponent<DisplayDamage>().showDamage(damage);
        damageObject.transform.SetParent(null);

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
            StartCoroutine(DoEnemyDeathAnimation());

            // gameObject.SetActive(false);
        }
    }

    IEnumerator DoEnemyDeathAnimation()
    {
        audioSource.PlayOneShot(cry, 0.7F);
        healthBar.SetActive(false);
        healthBarBackground.SetActive(false);
        shotCtrl.enabled = false;
        collider.enabled = false;
        animator.SetBool("IsDead", true);
        gameObject.tag = "DeadEnemy";
        isDead = true;
        //play sound
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
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
        if (message.Equals("EntryAnimationEnded") && !spawned)
        {
            spawned = true;
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
                other.gameObject.GetComponent<Player>().DealDamage(Mathf.RoundToInt(enemyStats.attackPower / 2), false);
            }
        }
    }
}
