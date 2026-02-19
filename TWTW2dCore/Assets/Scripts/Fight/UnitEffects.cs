using UnityEngine;

public class UnitEffects : MonoBehaviour
{
    private SpriteRenderer sr;

    public GameObject fireEffect; // сюда префаб огня

    public GameObject frostEffectPrefab;


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetFireVisual(bool state)
    {
        if (fireEffect != null)
            fireEffect.SetActive(state);
    }

    public void SetFreezeVisual(bool state)
    {
        if (frostEffectPrefab != null)
        {
            frostEffectPrefab.SetActive(state);
            sr.color = Color.cyan;
        }
    }

}
