using UnityEngine;
using UnityEngine.AI; // Required for NavMesh

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float DetectionRange = 10f;
    public Transform PlayerTransform;

    [Header("Patrol Settings")]
    public Transform[] Waypoints;
    private int currentWaypointIndex = 0;

    private NavMeshAgent agent; // Reference to the agent

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
    }

    private void Update()
    {
        if (PlayerTransform != null && Vector3.Distance(transform.position, PlayerTransform.position) <= DetectionRange)
        {
            agent.SetDestination(PlayerTransform.position);
        }
        else if (Waypoints.Length > 0)
        {
            Patrol();
        }
    }

    private void FindPlayer()
    {
        if (PlayerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) PlayerTransform = player.transform;
        }
    }

    private void Patrol()
    {
        // Set the destination to the current waypoint
        agent.SetDestination(Waypoints[currentWaypointIndex].position);

        // Check if we've arrived
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;
        }
    }
}