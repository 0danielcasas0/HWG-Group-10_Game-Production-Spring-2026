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
        if (PlayerStats.IsDetected && !wasPlayerSeen)
        {
            Normal.Stop();
            Seen.Play();
            wasPlayerSeen = true;
        }
        else if (!PlayerStats.IsDetected && wasPlayerSeen)
        {
            Seen.Stop();
            Normal.Play();
            wasPlayerSeen = false;
        }
    }
}
