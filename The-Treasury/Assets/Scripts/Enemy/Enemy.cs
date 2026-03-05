using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float DetectionRange = 10f;
    public float ViewAngle = 90f; 
    public float HearingRange = 7f; 
    public Transform PlayerTransform;
    public float CaughtTimer = 1f; 
    public float CatchDistance = 3f; 
    public LayerMask SightLayers = -1; 

    [Header("Patrol Settings")]
    public Transform[] Waypoints;
    private int CurrentWaypointIndex = 0;

    // References and internal states
    private PlayerStats PlayerStatsRef;
    private NavMeshAgent EnemyAgent;
    private Vector3 LastKnownLocation;
    private bool IsPlayerDetected;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStatsRef = player.GetComponent<PlayerStats>();
            PlayerTransform = player.transform;
        }

        EnemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        StealthCheck();
        SensePlayer(); 
        HandleMovement();
        Catch();
    }

    private void SensePlayer()
    {
        // Enemy is alerted if they see OR hear the player
        if (CanSeePlayer() || CanHearPlayer())
        {
            IsPlayerDetected = true;
            LastKnownLocation = PlayerTransform.position;
        }
        else
        {
            IsPlayerDetected = false;
        }
    }

    private bool CanSeePlayer()
    {
        if (PlayerTransform == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);
        
        if (distanceToPlayer <= DetectionRange)
        {
            Vector3 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
            float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleBetweenEnemyAndPlayer < ViewAngle / 2f)
            {
                Vector3 origin = transform.position + Vector3.up;
                Vector3 target = PlayerTransform.position + Vector3.up;

                if (Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, distanceToPlayer, SightLayers))
                {
                    if (hit.transform == PlayerTransform) return true;
                }
            }
        }
        return false;
    }

    private bool CanHearPlayer()
    {
        if (PlayerTransform == null || PlayerStatsRef == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);

        if (PlayerStatsRef.IsStealthy) return false;

        if (distanceToPlayer <= HearingRange)
        {
            return true;
        }

        return false;
    }

    private void HandleMovement()
    {
        if (IsPlayerDetected)
        {
            EnemyAgent.SetDestination(PlayerTransform.position);
        }
        else if (Waypoints.Length > 0)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        EnemyAgent.SetDestination(Waypoints[CurrentWaypointIndex].position);

        if (!EnemyAgent.pathPending && EnemyAgent.remainingDistance <= EnemyAgent.stoppingDistance)
        {
            CurrentWaypointIndex = (CurrentWaypointIndex + 1) % Waypoints.Length;
        }
    }

    private void Catch()
    {
        // REQUIREMENT: Must be within distance AND visible to be caught
        bool isWithinRange = PlayerTransform != null && Vector3.Distance(transform.position, PlayerTransform.position) <= CatchDistance;
        
        if (isWithinRange && CanSeePlayer())
        {
            CaughtTimer -= Time.deltaTime;
            
            if (CaughtTimer <= 0f)
            {
                PlayerStatsRef.IsCaught = true;
                Debug.Log("Player has been caught!");
            }
        }
        else
        {
            // Reset the timer if the player breaks line of sight or moves away
            CaughtTimer = 1f; 
        }
    }

    private void StealthCheck()
    {
        if (PlayerStatsRef != null && PlayerStatsRef.IsStealthy)
        {
            DetectionRange = 5f;
            HearingRange = 3f; 
        }
        else
        {
            DetectionRange = 10f;
            HearingRange = 7f;
        }
    }
}