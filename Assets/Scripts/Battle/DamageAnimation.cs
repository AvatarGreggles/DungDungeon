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

    public IEnumerator PlayDamageAnimation(GameObject damagedTarget)
    {
        bool isPlayer = damagedTarget.CompareTag("Player");
        Player player = damagedTarget.GetComponent<Player>();

        if (isPlayer)
        {
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
            // if (player != null)
            // {
            //     yield return new WaitForSeconds(player.invincibilityFrameTime);
            //     player.isInvincible = false;
            // }
        }
    }
}
