using UnityEngine;

public class Crate : MonoBehaviour, IInteractable
{
    public Transform HidePoint;
    public Transform UnhidePoint;

    private PlayerStats playerStats;
    private GameObject player;
    public AudioClip BarrelSound;


    public void Interact()
    {        Debug.Log("Interacting with crate...");

        // Get reference to PlayerStats on the player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerStats.IsHiding = !playerStats.IsHiding;  // Toggle hiding state

        // Resets player scale to normal height
        player.transform.localScale = new Vector3(1, 1, 1);

        // Play sound effect when playter hides or unhides
        if (BarrelSound != null)
        {
            AudioSource.PlayClipAtPoint(BarrelSound, player.transform.position);
        }


        if (playerStats.IsHiding)
        {
            if (HidePoint != null)
            {
                player.transform.position = HidePoint.position;
                player.transform.rotation = HidePoint.rotation;
                playerStats.IsStealthy = true;
            }
        }
        else
        {
            if (UnhidePoint != null)
            {
                player.transform.position = UnhidePoint.position;
                player.transform.rotation = UnhidePoint.rotation;
                playerStats.IsStealthy = false;
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
