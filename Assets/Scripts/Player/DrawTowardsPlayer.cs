using System.Collections;
using UnityEngine;

public class DrawTowardsPlayer : MonoBehaviour
{
    Player player;
    private bool inRange = false;

    Rigidbody2D _rigidbody;
    Collider2D _collider;

    SpriteRenderer spriteRenderer;

    [Range(0.75f, 2)]
    [SerializeField] float drawDelayTime = 2f;

    AudioSource audioSource;
    public AudioClip coinCollectSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.currentState == State.Cleared || GameController.Instance.currentState == State.LevelUp)
        {
            float dis = Vector3.Distance(player.transform.position, transform.position);
            if (dis < 30)
            {
                float speed = 30 - dis;
                speed = speed * Time.deltaTime * .5f;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HandleCollection());

        }
    }

    IEnumerator HandleCollection()
    {
        audioSource.PlayOneShot(coinCollectSound, 1f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

