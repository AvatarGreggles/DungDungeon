using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    float damageAnimationFramleDelay = 0.05f;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayDamageAnimation()
    {
        sprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 1, 1, 1);
    }
}
