using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitEffects : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void PlayHitEffect(DamageType type)
    {
        StopAllCoroutines();
        StartCoroutine(HitFlash(type));
    }

    IEnumerator HitFlash(DamageType type)
    {
        Color hitColor = Color.red;

        if (type == DamageType.Fire)
            hitColor = new Color(1f, 0.4f, 0f);

        if (type == DamageType.Ice)
            hitColor = new Color(0.5f, 0.8f, 1f);

        sr.color = hitColor;

        yield return new WaitForSeconds(0.1f);

        sr.color = originalColor;
    }
}
