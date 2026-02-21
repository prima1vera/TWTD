using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private Vector3 direction;
    public DamageType damageType = DamageType.Normal;
    public float knockbackForce = 0.2f;
    public TrailRenderer trail;
    private Rigidbody2D rb;

    public GameObject dustPrefab;
    private bool canImpact = false;

    public float impactRadius = 1.2f;
    public LayerMask unitLayer;
    public LayerMask groundLayer;

    void EnableImpact()
    {
        canImpact = true;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * speed;

        Invoke(nameof(EnableImpact), 0.05f);
    }

    void Impact()
    {
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        GetComponent<Collider2D>().enabled = false;

        if (dustPrefab != null)
        {
            Instantiate(dustPrefab, transform.position, Quaternion.identity);
            //dustPrefab.SetActive(true);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, impactRadius, unitLayer);

        foreach (Collider2D hit in hits)
        {
            UnitHealth health = hit.GetComponent<UnitHealth>();
            if (health == null) continue;

            Vector2 forceDir = (hit.transform.position - transform.position).normalized;

            health.TakeDamage(damage, damageType, forceDir, knockbackForce);

            StatusEffectHandler status = hit.GetComponent<StatusEffectHandler>();

            if (status != null)
            {
                if (damageType == DamageType.Fire)
                    status.ApplyBurn(3f, 1, 0.5f);

                if (damageType == DamageType.Ice)
                    status.ApplyFreeze(2f, 0.4f);
            }
        }

        Destroy(gameObject, 1.0f);
        //Destroy(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;

        if (trail != null)
        {
            switch (damageType)
            {
                case DamageType.Fire:
                    trail.startColor = new Color(1f, 0.3f, 0f, 0.8f);
                    break;

                case DamageType.Ice:
                    trail.startColor = new Color(0.3f, 0.8f, 1f, 0.8f);
                    break;

                default:
                    trail.startColor = new Color(1f, 1f, 1f, 0.6f);
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canImpact) return;

        Debug.Log("TRIGGER: " + other.name);

        int otherLayer = other.gameObject.layer;

        if (((1 << otherLayer) & groundLayer) != 0)
        {
            Impact();
            return;
        }

        if (((1 << otherLayer) & unitLayer) != 0)
        {
            Impact();
            return;
        }
    }
}
