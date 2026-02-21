using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region === Enemy Settings ===
    // Groups enemy-related variables in the Inspector
    [Header("Enemy Settings")]
    public float MoveSpeed = 3f;                 // Base movement speed of the enemy
    public float DetectionRange = 10f;           // Range at which the enemy detects the player
    public float AttackRange = 2f;              // Range at which the enemy can attack
    public int Health = 100;                    // Enemy health points
    public Transform PlayerTransform;           // Reference to the player's transform
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find the player in the scene and assign it to PlayerTransform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    // Method to handle enemy movement towards the player
    void MoveTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (PlayerTransform.position - transform.position).normalized;
        // Move the enemy towards the player
        transform.position += direction * MoveSpeed * Time.deltaTime;
    }
}
