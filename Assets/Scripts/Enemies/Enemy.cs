using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isSpawner = false;
    public float health;

    float initialHealth;
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
    public bool isSpawnee = false;


    public AudioClip cry;
    AudioSource audioSource;

    [SerializeField] GameObject targetIcon;


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
        healthBar.GetComponent<SpriteRenderer>().color = Color.green;

        targetIcon.SetActive(false);
    }

    void OnEnable()
    {
        health = enemyStats.maxHP;
    }

    public void SetAsTargetted()
    {
        targetIcon.SetActive(true);
    }

    public void Untarget()
    {
        targetIcon.SetActive(false);
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
        if (health < enemyStats.maxHP / 4)
        {
            healthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (gameObject.activeSelf)
        {
            bool isPlayer = gameObject.CompareTag("Player");
            StartCoroutine(GetComponent<DamageAnimation>().PlayDamageAnimation());
            healthBar.transform.localScale = new Vector3(initialHealthBarSize.x * (health / enemyStats.maxHP), initialHealthBarSize.y, initialHealthBarSize.z);
        }

        IsEnemyDead();
    }

    private void IsEnemyDead()
    {
        if (health <= 0)
        {
            Untarget();

            LevelManager.Instance.RemoveEnemy(gameObject);
            // GameController.Instance.AddCurrency(enemyStats.currencyDrop);
            DropLoot();
            GivePlayersExperience();

            StartCoroutine(DoEnemyDeathAnimation());


            // gameObject.SetActive(false);
        }
    }

    IEnumerator DoEnemyDeathAnimation()
    {
        if (!isSpawner)
        {
            audioSource.PlayOneShot(cry, 0.7F);
            shotCtrl.enabled = false;
            animator.SetBool("IsDead", true);
        }

        healthBar.SetActive(false);
        healthBarBackground.SetActive(false);

        collider.enabled = false;
        gameObject.tag = "DeadEnemy";
        isDead = true;
        //play sound
        if (!isSpawner)
        {
            yield return new WaitForSeconds(1.5f);
        }

        if (isSpawnee)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void GivePlayersExperience()
    {
        foreach (Player player in GameController.Instance.players)
        {
            player.HandlePlayerGains(enemyStats.expYield, enemyStats.currencyDrop);

        }
    }

    private void DropLoot()
    {

        GameObject newLoot = Instantiate(loot, transform.position, Quaternion.identity);

        newLoot.GetComponent<DrawTowardsPlayer>().value = enemyStats.currencyDrop;

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
