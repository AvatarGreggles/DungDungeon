using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;
    private Vector2 movement;
    private Rigidbody2D rbody;

    public Vector3 targetedEnemy;

    public Transform spawnPosition;

    public Animator playerAnimator;

    private bool isFacingRight = false;

    Vector2 originalDungScale;

    Player player;

    SpriteRenderer playerSprite;

    [SerializeField] PlayerInput playerInput;

    public float dungAccumulationRate = 0.1f;

    [SerializeField] Transform dustSpawnPointLeft;
    [SerializeField] Transform dustSpawnPointRight;

    [SerializeField] GameObject dustTrailObject;
    GameObject dustTrail;

    Vector3 newTrailRotation;

    Animator animator;

    public AudioClip dungCollectSound;
    AudioSource audioSource;

    PlayerBaseStatManager playerBaseStats;


    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerSprite = GetComponent<SpriteRenderer>();
        originalDungScale = transform.localScale;
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();

        dustTrail = Instantiate(dustTrailObject, dustSpawnPointLeft.position, Quaternion.identity);
        dustTrail.transform.SetParent(player.transform);

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerBaseStats = FindObjectOfType<PlayerBaseStatManager>();
        spawnPosition = LevelManager.Instance.playerSpawnPoint;


        newTrailRotation = dustTrail.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (targetedEnemy != null)
        {
            // Vector3 targetPos = Camera.main.ScreenToWorldPoint(targetedEnemy);
            // transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos - transform.position);

            // Raycast Mouse Looking
            Ray ray = Camera.main.ScreenPointToRay(targetedEnemy);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 hitPoint = hit.point;

                Vector3 targetDir = hitPoint - transform.position;

                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        // Vector3 newTrailRotation = dustTrail.transform.position;
        if (isFacingRight)
        {
            // Vector3 newScale = transform.localScale;
            // newScale.x = originalDungScale.x * 1f;
            // transform.localScale = newScale;
            dustTrail.transform.position = new Vector3(dustSpawnPointLeft.transform.position.x, dustSpawnPointLeft.transform.position.y, dustSpawnPointLeft.transform.position.z);
            dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, 90, newTrailRotation.z);
            playerSprite.flipX = false;
        }
        else
        {
            // Vector3 newScale = transform.localScale;
            // newScale.x = originalDungScale.x * -1f;
            // transform.localScale = newScale;
            dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, -90, newTrailRotation.z);

            dustTrail.transform.position = new Vector3(dustSpawnPointRight.transform.position.x, dustSpawnPointRight.transform.position.y, dustSpawnPointRight.transform.position.z);
            playerSprite.flipX = true;
        }


    }

    public IEnumerator CheckIfReleased(Vector2 movement)
    {
        yield return new WaitForSeconds(0.01f);
        if (new Vector2(0f, 0f) == movement)
        {
            PlayerAttack.Instance.attackReleased = true;
        }
        else
        {
            PlayerAttack.Instance.attackReleased = false;
        }

        yield return null;
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();

        StartCoroutine(CheckIfReleased(movement));

        if (movement.x < 0f)
        {
            // playerAnimator.SetBool("IsFacingRight", false);
            dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, 90, newTrailRotation.z);
            isFacingRight = false;
        }
        else if (movement.x > 0f)
        {
            // playerAnimator.SetBool("IsFacingRight", true);
            dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, -90, newTrailRotation.z);
            isFacingRight = true;
        }

        dustTrail.GetComponent<ParticleSystem>().Play();

    }



    private void Movement()
    {
        Vector2 currentPos = rbody.position;
        Vector2 adjustedMovement = movement * (movementSpeed + playerBaseStats.bonusMoveSpeed);
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;


        // Vector3 newPosWithRotation = new Vector3(newPos.x, newPos.y, targetedEnemy.z);
        if (newPos != new Vector2(transform.position.x, transform.position.y) && GameController.Instance.currentState != State.Cleared && player.dungAccumulated < player.maxDungSize)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(dungCollectSound, 0.75F);
            }
            player.AccumulateDung(dungAccumulationRate);

        }

        if (adjustedMovement == Vector2.zero)
        {
            animator.SetBool("IsMoving", false);
            dustTrail.GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            animator.SetBool("IsMoving", true);
        }


        rbody.MovePosition(newPos);
    }


    void FixedUpdate()
    {
        Movement();

    }

    public void ResetPosition()
    {
        if (transform)
        {
            spawnPosition = LevelManager.Instance.playerSpawnPoint.transform;
            transform.position = spawnPosition.position;
        }
    }
}
