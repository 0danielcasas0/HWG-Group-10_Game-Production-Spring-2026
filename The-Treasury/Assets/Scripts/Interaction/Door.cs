using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        // Check if the player has the key before allowing interaction
        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();
        if (playerStats != null && playerStats.HasKey)
        {
            // Logic to open the door, e.g., play animation, disable collider, etc.
            Debug.Log("Door opened!");
            // You can add your door opening logic here (e.g., play animation, disable collider, etc.)
            gameObject.SetActive(false);
            playerStats.HasKey = false; // Optionally consume the key after opening the door
        }
        else
        {
            Debug.Log("You need a key to open this door.");
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
