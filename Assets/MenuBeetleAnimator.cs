using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBeetleAnimator : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

    // The target (cylinder) position.
    [SerializeField] Transform startPoint;
    [SerializeField] Transform target;

    Transform currentTarget;

    bool isMovingRight = true;

    SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer dungSprite;

    // Start is called before the first frame update
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                isMovingRight = false;
                spriteRenderer.flipX = true;
                if (dungSprite != null)
                {
                    dungSprite.enabled = false;
                }
            }
        }
        else
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, startPoint.position) < 0.001f)
            {
                isMovingRight = true;
                spriteRenderer.flipX = false;
                if (dungSprite != null)
                {
                    dungSprite.enabled = true;
                }
            }
        }


    }
}
