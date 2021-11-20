using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    float damageAnimationFramleDelay = 0.05f;

    SpriteRenderer sprite;

    public float invincibilityTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayDamageAnimation(Collider2D collider, bool isPlayer, GameObject newGameObject)
    {

        if (isPlayer)
        {
            if (collider.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(newGameObject.GetComponent<Collider2D>(), collider, true);
            }
            // collider.enabled = false;
            sprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(damageAnimationFramleDelay);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(damageAnimationFramleDelay);
        }
        sprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(damageAnimationFramleDelay);
        sprite.color = new Color(1, 1, 1, 1);

        if (isPlayer)
        {
            sprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(damageAnimationFramleDelay);
            sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(invincibilityTime);

            if (collider.gameObject.tag == "Player")
            {
                Physics2D.IgnoreCollision(newGameObject.GetComponent<Collider2D>(), collider, false);
            }
            // collider.enabled = true;
        }
    }
}
