using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private Vector3 direction;
    public DamageType damageType = DamageType.Normal;
    public float knockbackForce = 0.2f;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnitHealth health = other.GetComponent<UnitHealth>();
        if (health != null)
        {
            Vector2 hitDirection = (other.transform.position - transform.position).normalized;
            health.TakeDamage(damage, damageType, hitDirection, knockbackForce);

            StatusEffectHandler status = other.GetComponent<StatusEffectHandler>();

            if (status != null)
            {
                if (damageType == DamageType.Fire)
                    status.ApplyBurn(3f, 1, 0.5f);

                if (damageType == DamageType.Ice)
                    status.ApplyFreeze(2f, 0.4f);
            }
        }
    }
}
