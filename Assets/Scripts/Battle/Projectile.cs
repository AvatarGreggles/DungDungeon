using UnityEngine;
using System.Linq;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 10f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)

    public float enemyAttackSpeed;

    public int power = 1;

    private Rigidbody2D m_Rigidbody;

    [SerializeField] GameObject damageSprite;
    [SerializeField] GameObject criticalDamageSprite;



    [SerializeField] bool isPlayerProjectile;

    public AudioClip shoot;
    public AudioClip hit;
    AudioSource audioSource;

    SpriteRenderer projectileSprite;

    bool shouldPlayerProjectilePassThroughWall = false;
    bool shouldPlayerProjectileBounce = false;

    public bool isCriticalHit = false;


    public enum projectileTypes
    {
        Default,
        SpreadShot,
    }

    [SerializeField] projectileTypes projectileType;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        projectileSprite = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        audioSource.PlayOneShot(shoot, 0.7F);
        if (isPlayerProjectile)
        {
            moveTowardsCloestTarget();
        }

        Destroy(gameObject, m_Lifespan);
    }

    public void SetScale(Player player)
    {
        projectileSprite.transform.localScale = new Vector3(player.dungSprite.transform.localScale.x, player.dungSprite.transform.localScale.y, 0f);
        player.ResetSpriteSize();
    }

    public void SetPower(int attack)
    {
        power = attack;
    }

    public void SetEnemyAttackSpeed(float speed)
    {

        enemyAttackSpeed = speed;

    }

    public void moveTowardsCloestTarget()
    {
        GameObject targetGO = null;
        Player player = GameController.Instance.players[0];
        if (player.targettableEnemies.Count > 0)
        {
            targetGO = player.targettableEnemies[player.currentTargetIndex];
        }
        else
        {
            Debug.Log("Not in range to shoot");
        }

        if (!targetGO)
        {
            Destroy(gameObject);
            return;
        }

        if (targetGO)
        {

            PlayerStatManager playerStatManager = player.GetComponent<PlayerStatManager>();
            var dir = targetGO.transform.position - transform.position;
            dir = dir.normalized;
            RotateTowardsTarget(targetGO.transform);

            if (targetGO.CompareTag("Enemy"))
            {
                m_Rigidbody.AddForce(dir * (m_Speed + playerStatManager.attackSpeedBonus));
            }

        }

    }

    // void SpreadShot()
    // {

    // }

    public GameObject FindClosestTarget(string targetTag)
    {
        //TODO copy the logic and use it t trigger a draw to player function for coins
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject closest = null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    // void ShowDamage(Enemy enemy, Player player)
    // {
    //     bool shootThroughEnemiesEnabled = FindObjectOfType<Player>().GetComponent<PlayerAbilities>().shootThroughEnemiesEnabled;
    //     if (enemy && shootThroughEnemiesEnabled)
    //     {
    //         //Do something
    //     }
    //     else
    //     {
    //         gameObject.SetActive(false);

    //     }
    // }

    private IEnumerator OnProjectileImpact(Enemy enemy = default, Player player = default)
    {
        if (player != null && player.isInvincible) { yield break; }
        if (isPlayerProjectile)
        {
            audioSource.PlayOneShot(hit, 0.7F);
        }
        // ShowDamage();
        if (enemy)
        {
            enemy.DealDamage(power, isCriticalHit);
        }

        if (player)
        {
            player.DealDamage(power, isCriticalHit);
        }

        // Clean up projectile
        PlayerAbilities currentPlayer = FindObjectOfType<Player>()?.GetComponent<PlayerAbilities>();
        if (currentPlayer && !currentPlayer.shootThroughEnemiesEnabled)
        {
            Destroy(gameObject);
        }

        yield return null;



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerProjectile)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (gameObject.activeSelf)
                {
                    StartCoroutine(OnProjectileImpact(other.gameObject.GetComponent<Enemy>(), null));
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (gameObject.activeSelf)
                {
                    StartCoroutine(OnProjectileImpact(null, other.gameObject.GetComponent<Player>()));
                }
            }
        }

        ShouldDestroyProjectileOnWallCollision(other.gameObject);
    }

    private void ShouldDestroyProjectileOnWallCollision(GameObject collidedObject)
    {
        // Clean up projectile
        Player currentPlayer = FindObjectOfType<Player>();
        if (collidedObject.CompareTag("Wall") && !shouldPlayerProjectilePassThroughWall)
        {
            if (isPlayerProjectile)
            {
                // GameController.Instance.currencyUI.ShowWallHitTip();
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }

    }

    public void EnablePassThroughWall()
    {
        shouldPlayerProjectilePassThroughWall = true;
    }

    public void EnableProjectileBounce()
    {
        shouldPlayerProjectileBounce = true;
    }

    private void RotateTowardsTarget(Transform target)
    {
        var offset = 90f;
        Vector2 direction = target.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }
}
