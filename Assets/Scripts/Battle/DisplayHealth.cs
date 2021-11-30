using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] float healthDisplayTime = 1f;
    [SerializeField] Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HideHealthDelay(healthDisplayTime));
    }

    IEnumerator HideHealthDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


    public void ShowHealth(float damage)
    {
        healthText.text = damage.ToString();
    }
}
