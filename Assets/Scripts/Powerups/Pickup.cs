using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public int cost = 1;


    public enum abilities
    {
        RapidShotEnabled,
        BurstShotEnabled,
        AttackUp,
        HealthUp
    }

    [SerializeField] abilities ability;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameController.Instance.totalCurrency >= cost)
            {
                switch (ability)
                {
                    case abilities.RapidShotEnabled:
                        other.gameObject.GetComponent<PlayerAbilities>().RapidShotEnabled();
                        break;
                    case abilities.BurstShotEnabled:
                        other.gameObject.GetComponent<PlayerAbilities>().BurstShotEnabled();
                        break;
                    case abilities.HealthUp:
                        other.gameObject.GetComponent<PlayerAbilities>().HealthUpEnabled();
                        break;
                    default:
                        break;

                }
                GameController.Instance.AddCurrency(-cost);
                // Destroy(gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough money!");
            }
        }
    }
}
