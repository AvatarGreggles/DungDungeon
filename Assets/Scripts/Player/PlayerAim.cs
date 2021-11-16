using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Vector3 target;
    PlayerMovement player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // LookAt 2D
            player.targetedEnemy = other.gameObject.transform.position;
        }
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     player.targetedEnemy = Vector3.zero;
    // }
}
