using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCollider : MonoBehaviour
{

    [SerializeField] int damage = 300;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.activeSelf)
            {
                other.gameObject.GetComponent<Player>().DealDamage(damage, false);
            }
        }
    }
}
