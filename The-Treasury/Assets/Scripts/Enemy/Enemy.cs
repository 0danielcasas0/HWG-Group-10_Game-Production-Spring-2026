using UnityEngine;
using UnityEngine.AI; // Required for NavMesh

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float DetectionRange = 10f;
    public Transform PlayerTransform;
    public float CaughtTimer = 1f; // Time in seconds the player needs to be within range to be caught
    public float CatchDistance = 3f; // Distance at which the player is considered caught

    [Header("Patrol Settings")]
    public Transform[] Waypoints;
    private int currentWaypointIndex = 0;

    // Reference to the Player's stats for potential future use (e.g. checking if player is caught)
    private PlayerStats playerStats;

    private NavMeshAgent agent; // Reference to the agent

    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        FindPlayer();
        FindPlayerStats();
    }

    private void Update()
    {
        Chase();
        Hearing();
        Catch();
        Grabbing();
    }

    private void FindPlayer()
    {
        if (PlayerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) PlayerTransform = player.transform;
        }
    }

    public void FindPlayerStats()
    {
        if (playerStats == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerStats = player.GetComponent<PlayerStats>();
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

    private void Chase()
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

    public void Hearing()
    {
        // If the player IsStealthy, the enemy should have a reduced detection range or ignore the player entirely
        if (playerStats.IsStealthy == true)
        {
            DetectionRange = 3f; // Example: reduce detection range to half when player is stealthy
        }
        else 
        {
            DetectionRange = 7f; // Reset detection range to default value
        }
    }

    public void Catch()
    {
        // Check if the enemy has reached within 2 units of the player for CaughtTimer to trigger the caught state
        if (PlayerTransform != null && Vector3.Distance(transform.position, PlayerTransform.position) <= CatchDistance)
        {
            CaughtTimer -= Time.deltaTime;
            if (CaughtTimer <= 0f)
            {
                playerStats.IsCaught = true; // Set the player's caught state to true
                Debug.Log("Player has been caught!");
            }
        }
        else
        {
            CaughtTimer = 3f; // Reset the timer if the player moves out of range
        }
    }

    public void Grabbing()
    {
        // This method will slow the player if they are within a certain range of the enemy, simulating the grabbing mechanic.
        if (PlayerTransform != null && Vector3.Distance(transform.position, PlayerTransform.position) <= CatchDistance)
        {
            playerStats.IsGrabbed = true; // Set the player's grabbed state to true
            Debug.Log("Player has been grabbed!");
        }
        else
        {
            playerStats.IsGrabbed = false; // Reset the grabbed state if the player moves out of range
        }
    }
}