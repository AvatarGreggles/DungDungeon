using UnityEngine;
using System.Linq;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 10f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)

    public int power = 1;

    private Rigidbody2D m_Rigidbody;

    [SerializeField] GameObject damageSprite;

    [SerializeField] bool isPlayerProjectile;

    public AudioClip shoot;
    AudioSource audioSource;

    SpriteRenderer projectileSprite;

    bool shouldPlayerProjectilePassThroughWall = false;
    bool shouldPlayerProjectileBounce = false;

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

    public void moveTowardsCloestTarget()
    {
        GameObject targetGO;

        if (isPlayerProjectile)
        {
            targetGO = FindClosestTarget("Enemy");
        }
        else
        {
            targetGO = FindClosestTarget("Player");
        }

        if (!targetGO)
        {
            Destroy(gameObject);
            return;
        }


        if (targetGO)
        {
            GameObject sourcePlayer = FindClosestTarget("Player");
            Player player = sourcePlayer.GetComponent<Player>();
            var dir = targetGO.transform.position - transform.position;
            dir = dir.normalized;
            RotateTowardsTarget(targetGO.transform);
            m_Rigidbody.AddForce(dir * (m_Speed * player.attackSpeedBonus));
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

    void ShowDamage(Enemy enemy, Player player)
    {
        GameObject target = isPlayerProjectile ? enemy.gameObject : player.gameObject;
        Transform damageDisplayPivot = isPlayerProjectile ? enemy.damageDisplayPivot.transform : player.damageDisplayPivot.transform;

        GameObject damageObject = Instantiate(damageSprite, damageDisplayPivot.position, damageDisplayPivot.rotation);
        damageObject.transform.SetParent(target.transform);
        damageObject.GetComponent<DisplayDamage>().showDamage(power);

        gameObject.SetActive(false);
        damageObject.transform.SetParent(null);
    }

    void HandleDamageDealing(Enemy enemy, Player player)
    {
        if (enemy)
        {
            enemy.DealDamage(power);
        }

        if (player)
        {
            player.DealDamage(power);
        }
    }

    private IEnumerator OnProjectileImpact(Enemy enemy = default, Player player = default)
    {
        ShowDamage(enemy, player);
        HandleDamageDealing(enemy, player);

        // Clean up projectile
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
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
        if (collidedObject.CompareTag("Wall") && !shouldPlayerProjectilePassThroughWall && !shouldPlayerProjectileBounce)
        {
            Destroy(gameObject);
        }

        if (collidedObject.CompareTag("Wall") && shouldPlayerProjectileBounce)
        {
            m_Rigidbody.gravityScale = 1f;

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
