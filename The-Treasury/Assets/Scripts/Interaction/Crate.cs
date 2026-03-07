using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    private PlayerStats playerStats;
    private GameObject player;


    public void Interact()
    {
        // Get reference to PlayerStats on the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerStats.IsHiding = !playerStats.IsHiding;

        if (playerStats.IsHiding)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
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
