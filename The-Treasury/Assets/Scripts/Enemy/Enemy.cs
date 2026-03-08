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

    private bool isCatching = false;

    private enum EnemyState { Patrol, Searching, Chase }
    private EnemyState currentState = EnemyState.Patrol;
    private Vector3 lastSeenPosition;
    private Vector3 lastHeardPosition;
    private bool wasChasingDueToSight = false;
    private Vector3 searchTarget;

    private float searchTimer = 0f;
    private float searchDuration = 3f; // Time to spend searching before returning to patrol
    private bool isSearching = false;
    private float lookAngle = 45f;
    private float lookSpeed = 2f;
    private float lookTime = 0f;
    private float originalRotationY;

    private bool previousIsHiding = false;
    private bool wasChasingWhenHidingStarted = false;

    private float waypointTimer = 0f;
    private float maxWaypointTime = 10f; // Time before considering stuck
    public bool isStuck = false;
    private Vector3 roamTarget;
    private float roamTimer = 0f;
    private float maxRoamTime = 5f; // Time to spend roaming
    private float roamRadius = 5f; // Distance to roam

    private float searchWaypointTimer = 0f;

    private void Start()
    {
        // Get reference to PlayerStats on the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = PatrolSpeed;
        FindPlayer();

        ResetCaughtTimer = CaughtTimer;
        // Look around
        originalRotationY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        StealthCheck();
        UpdateState();
        Move();
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
        if (isStuck)
        {
            // Roaming behavior
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Finished roaming, try waypoint again
                isStuck = false;
                waypointTimer = 0f;
                roamTimer = 0f;
            }
            else
            {
                roamTimer += Time.deltaTime;
                if (roamTimer > maxRoamTime)
                {
                    // Roam timeout, try waypoint again
                    isStuck = false;
                    waypointTimer = 0f;
                    roamTimer = 0f;
                }
            }
        }
        else
        {
            // Normal patrol to waypoint
            agent.SetDestination(Waypoints[currentWaypointIndex].position);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Arrived at waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Length;
                waypointTimer = 0f;
            }
            else
            {
                waypointTimer += Time.deltaTime;
                if (waypointTimer > maxWaypointTime)
                {
                    // Stuck, start roaming
                    isStuck = true;
                    roamTimer = 0f;
                    // Set random roam target
                    Vector3 randomDir = Random.insideUnitSphere * roamRadius;
                    randomDir.y = 0;
                    roamTarget = transform.position + randomDir;
                    agent.SetDestination(roamTarget);
                }
            }
        }
    }

    private void UpdateState()
    {
        // Check if hiding state changed
        if (playerStats.IsHiding != previousIsHiding)
        {
            if (playerStats.IsHiding)
            {
                wasChasingWhenHidingStarted = (currentState == EnemyState.Chase);
            }
        }
        previousIsHiding = playerStats.IsHiding;

        bool canSeePlayer = false;
        if (Vision != null)
        {
            canSeePlayer = Vision.CanSeePlayer;
        }

        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
        bool inDetectionRange = distance <= DetectionRange;
        bool canDetectByRange = inDetectionRange && (playerStats.IsMoving || isCatching);

        // If hiding and hiding didn't start during chase, disable all detection
        if (playerStats.IsHiding && !wasChasingWhenHidingStarted)
        {
            canSeePlayer = false;
            canDetectByRange = false;
        }

        if (canDetectByRange || canSeePlayer)
        {
            currentState = EnemyState.Chase;
            if (canSeePlayer)
            {
                wasChasingDueToSight = true;
                lastSeenPosition = PlayerTransform.position;
            }
            else if (canDetectByRange)
            {
                wasChasingDueToSight = false;
                lastHeardPosition = PlayerTransform.position;
            }
            playerStats.PlayerSeen = true;
        }
        else
        {
            if (currentState == EnemyState.Chase)
            {
                currentState = EnemyState.Searching;
                // Set search target based on what caused the chase
                if (wasChasingDueToSight)
                {
                    searchTarget = lastSeenPosition;
                }
                else
                {
                    searchTarget = lastHeardPosition;
                }
            }
            playerStats.PlayerSeen = false;
        }
    }

    private void Move()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                agent.speed = PatrolSpeed;
                break;
            case EnemyState.Searching:
                Search();
                agent.speed = PatrolSpeed;
                break;
            case EnemyState.Chase:
                agent.SetDestination(PlayerTransform.position);
                agent.speed = ChaseSpeed;
                break;
        }
    }

    private void Search()
    {
        agent.SetDestination(searchTarget);
        // Check if arrived
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isSearching)
            {
                isSearching = true;
                searchTimer = searchDuration;
                lookTime = 0f;
                originalRotationY = transform.eulerAngles.y; // Update facing direction for looking
            }
            LookAround();
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                currentState = EnemyState.Patrol;
                isSearching = false;
            }
        }
        else
        {
            searchWaypointTimer += Time.deltaTime;
            if (searchWaypointTimer > maxWaypointTime)
            {
                // Stuck, go back to patrolling
                currentState = EnemyState.Patrol;
            }
            isSearching = false;
        }
    }

    private void LookAround()
    {
        lookTime += Time.deltaTime * lookSpeed;
        float angle = Mathf.Sin(lookTime) * lookAngle;
        transform.rotation = Quaternion.Euler(0, originalRotationY + angle, 0);
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
            isCatching = true;
            CaughtTimer -= Time.deltaTime;
            if (CaughtTimer <= 0f)
            {
                playerStats.IsCaught = true; // Set the player's caught state to true
                Debug.Log("Player has been caught!");
            }
        }
        else
        {
            isCatching = false;
            CaughtTimer = ResetCaughtTimer; // Reset the timer if the player moves out of range
        }
    }
}