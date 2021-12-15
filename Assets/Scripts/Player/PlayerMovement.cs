using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    public float movementSpeed;
    private Vector2 movement;
    private Rigidbody2D rbody;

    public Vector3 targetedEnemy;

    public Transform spawnPosition;

    public Animator playerAnimator;

    private bool isFacingRight = false;

    Vector2 originalDungScale;

    Player player;

    PlayerStatManager playerStatManager;

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
        playerStatManager = GetComponent<PlayerStatManager>();
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

    }

    void FixedUpdate()
    {
        Movement();
    }




    void OnMove(InputValue value)
    {
        if (GameController.Instance.currentState != State.Dialog)
        {
            movement = value.Get<Vector2>();
            if (movement.x > 0f)
            {

                dustTrail.transform.position = new Vector3(dustSpawnPointLeft.transform.position.x, dustSpawnPointLeft.transform.position.y, dustSpawnPointLeft.transform.position.z);
                dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, 90, newTrailRotation.z);
                playerSprite.flipX = false;
            }
            else if (movement.x < 0f)
            {

                dustTrail.transform.eulerAngles = new Vector3(newTrailRotation.x, -90, newTrailRotation.z);
                dustTrail.transform.position = new Vector3(dustSpawnPointRight.transform.position.x, dustSpawnPointRight.transform.position.y, dustSpawnPointRight.transform.position.z);
                playerSprite.flipX = true;
            }

            dustTrail.GetComponent<ParticleSystem>().Play();
        }
    }


    private void Movement()
    {
        Vector2 currentPos = rbody.position;
        Vector2 adjustedMovement = movement * (movementSpeed + playerBaseStats.bonusMoveSpeed);
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;


        if (adjustedMovement != Vector2.zero)
        {
            if (newPos != new Vector2(transform.position.x, transform.position.y) && GameController.Instance.currentState != State.Cleared && playerStatManager.dungAccumulated < playerStatManager.maxDungSize)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(dungCollectSound, 0.75F);
                }
                player.AccumulateDung(dungAccumulationRate);
                animator.SetBool("IsMoving", true);
            }
        }
        else
        {
            OnStopMovement();

        }

        rbody.MovePosition(newPos);
    }

    void OnStopMovement()
    {
        animator.SetBool("IsMoving", false);
        dustTrail.GetComponent<ParticleSystem>().Stop();
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
