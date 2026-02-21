using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float range = 5f;
    public float fireRate = 1f;
    public Transform firePoint;

    private Transform currentTarget;
    private float fireCountdown = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        fireCountdown -= Time.deltaTime;

        Transform target = FindTarget();


        if (target == null)
        {
            currentTarget = null;
            return;
        }

        currentTarget = target;

        if (fireCountdown <= 0f)
        {
            animator.SetTrigger("Shoot");
            fireCountdown = 1f / fireRate;
        }
    }

    Transform FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float shortest = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            UnitHealth health = enemy.GetComponent<UnitHealth>();
            if (health != null && health.CurrentState != UnitState.Dead)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < shortest && dist <= range)
                {
                    shortest = dist;
                    nearest = enemy.transform;
                }
            }
        }

        return nearest;
    }

    public void ShootArrow()
    {
        if (currentTarget == null) return;

        GameObject arrowGO = Instantiate(
            arrowPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Arrow arrow = arrowGO.GetComponent<Arrow>();
        if (arrow != null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 targetPoint = (Vector2)currentTarget.position + randomOffset;

            arrow.Launch(targetPoint);
        }
    }
}
