using UnityEngine;

public class Gold : MonoBehaviour, IInteractable
{
    public bool IsFloating = true; // Option to enable or disable floating animation
    private Vector3 startPos; // Store the initial position of the gold for animation purposes

    public void Interact()
    {
        Debug.Log("You Win The Gold Is Collected!");
        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
        playerStats.HasGold = true;
        Destroy(gameObject); // Destroy the gold object after collection
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Record where the object started
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFloating)
        {
            transform.Rotate(Vector3.up, 50 * Time.deltaTime);
            float bounce = Mathf.Sin(Time.time * 2) * 0.1f;
            transform.position = new Vector3(startPos.x, startPos.y + bounce, startPos.z);
        }
    }
}
