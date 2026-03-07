using UnityEngine;
using UnityEngine.AI; // Required for NavMesh

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float DetectionRange = 5f;
    public Transform PlayerTransform;
    public EnemyVision Vision;
    private float ResetCaughtTimer = 1f;
    public float CaughtTimer = 1f; // Time in seconds the player needs to be within range to be caught
    public float CatchDistance = 2f; // Distance at which the player is considered caught

    [Header("Speed Settings")]
    public float PatrolSpeed = 2f;
    public float ChaseSpeed = 5f;

    [Header("Patrol Settings")]
    public Transform[] Waypoints;
    private int currentWaypointIndex = 0;

    // Reference to the Player's stats for potential future use (e.g. checking if player is caught)
    private PlayerStats playerStats;

    private NavMeshAgent agent; // Reference to the agent

    private void Start()
    {
        // Get reference to PlayerStats on the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = PatrolSpeed;
        FindPlayer();

        ResetCaughtTimer = CaughtTimer;
    }

    private void Update()
    {
        StealthCheck();
        Chase();
        Catch();
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

    private void Chase()
    {
        bool canSeePlayer = false;
        if (Vision != null)
        {
            canSeePlayer = Vision.CanSeePlayer;
        }

        if (PlayerTransform != null && (Vector3.Distance(transform.position, PlayerTransform.position) <= DetectionRange || canSeePlayer))
        {
            agent.SetDestination(PlayerTransform.position);
            agent.speed = ChaseSpeed;
            playerStats.PlayerSeen = true;
        }
        else
        {
            agent.speed = PatrolSpeed;
            playerStats.PlayerSeen = false;
            if (Waypoints.Length > 0)
            {
                Patrol();
            }
        }
    }

    private void StealthCheck()
    {
        // If the player IsStealthy, the enemy should have a reduced detection range or ignore the player entirely
        if (playerStats.IsStealthy == true)
        {
            DetectionRange = 3f; // Example: reduce detection range to half when player is stealthy
        }
        else 
        {
            DetectionRange = 5f; // Reset detection range to default value
        }
    }

    private void Catch()
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
            CaughtTimer = ResetCaughtTimer; // Reset the timer if the player moves out of range
        }
    }
}