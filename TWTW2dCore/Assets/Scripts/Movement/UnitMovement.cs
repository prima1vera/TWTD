using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float speed = 2f;

    private int waypointIndex = 0;
    private UnitHealth unitHealth;
    private Transform[] path;

    void Start()
    {
        unitHealth = GetComponent<UnitHealth>();

        int pathIndex = Random.Range(0, Waypoints.AllPaths.Length);
        path = Waypoints.AllPaths[pathIndex];

        float yOffset = Random.Range(-5f, 5f);
        transform.position += new Vector3(0, yOffset, 0);
    }

    void Update()
    {
        if (path == null || path.Length == 0 || unitHealth.IsDead) return;

        Transform target = path[waypointIndex];
        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

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
