using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{


    [SerializeField] Transform startPoint;
    [SerializeField] Transform target;
    bool isTriggered = false;
    bool isRetracting = false;

    [SerializeField] Transform enemy;

    [SerializeField] float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            if (!isRetracting)
            {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                enemy.position = Vector3.MoveTowards(enemy.position, target.position, step);

                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(enemy.position, target.position) < 0.001f)
                {
                    isRetracting = true;
                }
            }
            else
            {
                float step = (speed / 2) * Time.deltaTime; // calculate distance to move
                enemy.position = Vector3.MoveTowards(enemy.position, startPoint.position, step);
                if (Vector3.Distance(enemy.position, startPoint.position) < 0.001f)
                {
                    isRetracting = false;
                    isTriggered = false;
                }
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }
}
