using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    public Transform HidePoint;
    public Transform UnhidePoint;

    private PlayerStats playerStats;
    private GameObject player;


    public void Interact()
    {        Debug.Log("Interacting with crate...");

        // Get reference to PlayerStats on the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerStats.IsHiding = !playerStats.IsHiding;  // Toggle hiding state


        if (playerStats.IsHiding)
        {
            if (HidePoint != null)
            {
                player.transform.position = HidePoint.position;
                player.transform.rotation = HidePoint.rotation;
            }
        }
        else
        {
            if (UnhidePoint != null)
            {
                player.transform.position = UnhidePoint.position;
                player.transform.rotation = UnhidePoint.rotation;
            }
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
