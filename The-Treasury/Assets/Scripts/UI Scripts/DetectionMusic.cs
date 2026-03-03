using Unity.VisualScripting;
using UnityEngine;

public class DetectionMusic : MonoBehaviour
{
    public AudioSource Normal;
    public AudioSource Seen;

    private bool wasPlayerSeen;

    public PlayerStats PlayerStats;

    void Start()
    {
        Normal.Play();
        Seen.Stop();
        wasPlayerSeen = false;
    }
    void Update()
    {
        if (PlayerStats.PlayerSeen && !wasPlayerSeen)
        {
            Normal.Stop();
            Seen.Play();
            wasPlayerSeen = true;
        }
        else if (!PlayerStats.PlayerSeen && wasPlayerSeen)
        {
            Seen.Stop();
            Normal.Play();
            wasPlayerSeen = false;
        }
    }
}
