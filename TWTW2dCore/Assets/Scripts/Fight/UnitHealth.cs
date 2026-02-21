using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    public UnitState CurrentState { get; private set; } = UnitState.Moving;

    private Animator animator;
    private Collider2D col;
    public GameObject bloodPoolPrefab;
    public GameObject bloodSplashPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int dmg, DamageType type, Vector2 hitDirection, float knockbackForce)
    {
        if (CurrentState == UnitState.Dead) return;

        currentHealth -= dmg;

        GetComponent<UnitMovement>().ApplyKnockback(hitDirection, knockbackForce);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakePureDamage(int dmg)
    {
        if (CurrentState == UnitState.Dead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }


    void Die()
    {
        CurrentState = UnitState.Dead;

        if (animator != null)
            animator.SetBool("isDead", true);

        if (col != null)
            col.enabled = false;

        Instantiate(bloodSplashPrefab, transform.position, Quaternion.identity);

        if (bloodPoolPrefab != null)
        {
            GameObject blood = Instantiate(bloodPoolPrefab, transform.position, Quaternion.identity);

            float scale = UnityEngine.Random.Range(0.5f, 1f);
            blood.transform.localScale = Vector3.one * scale;

            //blood.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            //Color c = sr.color;
            //c = Color.Lerp(c, Color.gray, 0.4f); // убираем насыщенность
            //c *= 0.8f; // затемняем
            //sr.color = c;
            sr.sortingLayerName = "Units_Dead";
            sr.sortingOrder = 0;
        }

        TopDownSorter sorter = GetComponent<TopDownSorter>();
        if (sorter != null)
            sorter.enabled = false;
    }

    public void SetState(UnitState newState)
    {
        if (CurrentState == UnitState.Dead) return;

        CurrentState = newState;
    }
}
