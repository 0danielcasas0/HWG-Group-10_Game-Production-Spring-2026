using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    // Reference to the PlayerStats component to update when the key is collected
    private PlayerStats playerStats;
    public bool IsFollowingEnemy = false; // Option to make the key follow the waypoints of an enemy
    public bool IsFloating = true; // Option to enable or disable floating animation
    private Vector3 startPos; // Store the initial position of the key for animation purposes
    public Transform FollowPoint; // Reference to the enemy's transform for following

    public void Interact()
    {
        if (playerStats != null)
        {
            playerStats.HasKey = true; // Update the player's stats to indicate they have the key
            Destroy(gameObject); // Destroy the key object after collection
        }

        // If the key is set to follow an enemy, start following
        if (IsFollowingEnemy == true)
        {
            FollowEnemy(FollowPoint);
        }
    }

    void Start()
    {
        // Use FindAnyObjectByType for better performance if only one instance exists
        playerStats = FindAnyObjectByType<PlayerStats>();

        // Record where the object started
        startPos = transform.position;
    }

    void Update()
    {
        // If we are following an enemy, we stop the floating logic entirely
        if (IsFollowingEnemy && FollowPoint != null)
        {
            FollowEnemy(FollowPoint);
            return; // Exit the method so the floating logic below doesn't run
        }

        if (IsFloating)
        {
            transform.Rotate(Vector3.up, 50 * Time.deltaTime);
            float bounce = Mathf.Sin(Time.time * 2) * 0.1f;
            transform.position = new Vector3(startPos.x, startPos.y + bounce, startPos.z);
        }
    }

    void FollowEnemy(Transform FollowPoint)
    {
        // Make the key follow the enemy's position
        transform.position = FollowPoint.position + Vector3.up * 0.5f; // Adjust the offset as needed
        // Copy the rotation of the enemy to make it look like it's attached
        transform.rotation = FollowPoint.rotation;
        
    }

}
