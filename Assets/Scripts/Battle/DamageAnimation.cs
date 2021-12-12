using System.Collections;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    float damageAnimationFramleDelay = 0.05f;
    SpriteRenderer sprite;
    public float invincibilityTime = 1f;
    public float numberOfDamageFrames = 5;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayDamageAnimation()
    {
        for (var i = 0; i < numberOfDamageFrames; i++)
        {
            sprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(damageAnimationFramleDelay);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(damageAnimationFramleDelay);
        }
    }
}
