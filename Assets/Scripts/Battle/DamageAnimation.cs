using System.Collections;
using UnityEngine;

public class DamageAnimation : MonoBehaviour
{
    float damageAnimationFramleDelay = 0.05f;
    SpriteRenderer sprite;

    [Header("Number of frames")]
    public float numberOfDamageFrames = 5;

    [Header("Colors")]
    public Color applyColor = new Color(1, 0, 0, 1);
    public Color defaultColor = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator PlayDamageAnimation()
    {
        for (var i = 0; i < numberOfDamageFrames; i++)
        {
            sprite.color = applyColor;
            yield return new WaitForSeconds(damageAnimationFramleDelay);
            sprite.color = defaultColor;
            yield return new WaitForSeconds(damageAnimationFramleDelay);
        }
    }
}
