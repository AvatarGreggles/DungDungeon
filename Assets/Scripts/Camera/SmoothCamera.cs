using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{


    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;


    private void Awake()
    {

    }

    private void Update()
    {
        Player[] Players = GameObject.FindObjectsOfType<Player>();

        if (Players.Length > 0)
        {
            target = Players[0].transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), destination, ref velocity, dampTime);
        }

    }
}
