using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float latestDirectionChangeTime;
    [SerializeField] float directionChangeTime = 3f;
    [SerializeField] float characterVelocity = 2f;
    private Vector2 movementDirection;
    private Vector2 movementPerSecond;

    [SerializeField] SpriteRenderer enemySprite;

    Enemy enemy;


    void Start()
    {
        enemy = GetComponent<Enemy>();
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
    }

    void calcuateNewMovementVector()
    {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy
        movementDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * (characterVelocity + (LevelManager.Instance.floor * 0.10f));

        if (movementPerSecond.x < 0)
        {
            enemySprite.flipX = true;
        }
        else if (movementPerSecond.x > 0)
        {
            enemySprite.flipX = false;
        }
    }

    void Update()
    {
        if (GameController.Instance.currentState != State.Paused && !enemy.isDead)
        {
            //if the changeTime was reached, calculate a new movement vector
            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                calcuateNewMovementVector();
            }

            //move enemy: 
            transform.position = new Vector2(transform.position.x + (movementPerSecond.x * Time.deltaTime),
            transform.position.y + (movementPerSecond.y * Time.deltaTime));
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Checks if enemy collided with wall or pit
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Pit"))
        {
            calcuateNewMovementVector();
        }
    }
}
