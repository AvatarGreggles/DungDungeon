using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] float damageDisplayTime = 1f;
    [SerializeField] Text damageText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(hideDamageDelay(damageDisplayTime));
    }

    IEnumerator hideDamageDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


    public void showDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
