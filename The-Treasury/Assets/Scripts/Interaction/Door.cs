using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject Hinge;
    public bool IsLocked = true;
    public bool IsOpen = false;

    // Door Audiosources
    public AudioSource DoorOpeningSound;
    public AudioSource DoorClosingSound;
    public AudioSource DoorLockedSound;

    public void Interact()
    {
        // Check if the player has the key before allowing interaction
        PlayerStats playerStats = FindAnyObjectByType<PlayerStats>();

        if (IsLocked)
        {
            if (playerStats != null && playerStats.HasKey)
            {
                DoorOpeningSound.Play();
                IsLocked = false;
                playerStats.HasKey = false; // Consume key only when unlocking
                Debug.Log("Door unlocked!");
            }
            else
            {
                DoorLockedSound.Play();
                Debug.Log("You need a key to unlock this door.");
            }
        }
        else
        {
            IsOpen = !IsOpen;
            if (Hinge != null)
            {
                float angle = IsOpen ? 90f : -90f;
                Hinge.transform.Rotate(Vector3.up, angle);
                if (IsOpen == false)
                {
                    DoorClosingSound.Play();
                }
                else                
                {
                    DoorOpeningSound.Play();
                }

            }
            Debug.Log(IsOpen ? "Door opened!" : "Door closed!");
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
