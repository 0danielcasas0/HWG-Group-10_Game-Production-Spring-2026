using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public bool CanSeePlayer { get; set; } 
    public string PlayerTag = "Player";

    private Transform playerTransform;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerTransform = other.transform;
            CheckLineOfSight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            playerTransform = null;
            CanSeePlayer = false;
        }
    }

    private void CheckLineOfSight()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Offset the start position to avoid hitting the enemy's own collider
        float offset = 1f;
        Vector3 start = transform.position + direction * offset;
        float adjustedDistance = distance - offset;

        Vector3 endPoint;
        if (adjustedDistance > 0 && Physics.Raycast(start, direction, out RaycastHit hit, adjustedDistance))
        {
            endPoint = hit.point;
            if (hit.transform.CompareTag(PlayerTag))
            {
                CanSeePlayer = true;
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else
        {
            endPoint = transform.position + direction * distance;
            CanSeePlayer = false;
        }

        // Visualize the raycast
        Color rayColor = CanSeePlayer ? Color.green : Color.red;
        Debug.DrawRay(transform.position, endPoint - transform.position, rayColor);
    }
}
