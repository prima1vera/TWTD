using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 2f;

    private int waypointIndex = 0;
    private UnitHealth unitHealth;
    private Transform[] path;
    private Animator animator;

    void Start()
    {
        unitHealth = GetComponent<UnitHealth>();
        animator = GetComponent<Animator>();

        int pathIndex = Random.Range(0, Waypoints.AllPaths.Length);
        path = Waypoints.AllPaths[pathIndex];

        float yOffset = Random.Range(-5f, 5f);
        transform.position += new Vector3(0, yOffset, 0);
    }

    void Update()
    {
        if (path == null || path.Length == 0) return;

        if (unitHealth.CurrentState != UnitState.Moving)
            return;

        Transform target = path[waypointIndex];
        Vector3 dir = target.position - transform.position;

        Vector3 movement = dir.normalized * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        //UPDATE PARAMS FOR BLEND TREE
        if (animator != null)
        {
            Vector2 direction = dir.normalized;
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex >= path.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
