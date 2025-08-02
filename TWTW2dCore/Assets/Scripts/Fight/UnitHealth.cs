using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;

    public int maxHealth = 1;
    private int currentHealth;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        if (IsDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        IsDead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
    }
}
