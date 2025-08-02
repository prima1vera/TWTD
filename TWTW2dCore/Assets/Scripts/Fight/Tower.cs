using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float range = 5f;
    public float fireRate = 1f;

    private float fireCountdown = 0f;

    void Update()
    {
        fireCountdown -= Time.deltaTime;

        GameObject target = FindTarget();
        if (target != null && fireCountdown <= 0f)
        {
            Fire(target.transform);
            fireCountdown = 1f / fireRate;
        }
    }

    GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float shortest = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            UnitHealth health = enemy.GetComponent<UnitHealth>();
            if (health != null && !health.IsDead)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < shortest && dist <= range)
                {
                    shortest = dist;
                    nearest = enemy;
                }
            }
        }

        return nearest;
    }

    void Fire(Transform target)
    {
        GameObject arrowGO = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrow = arrowGO.GetComponent<Arrow>();

        if (arrow != null)
        {
            Vector3 dir = target.position - transform.position;
            arrow.SetDirection(dir);
        }
    }
}
