using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Player stats
    public bool HasGold = false;
    public bool IsCaught = false;
    public bool HasKey = false;
    public bool IsStealthy = false;
    public bool PlayerSeen = false;
    public bool IsHiding = false;
    public bool IsMoving = false;

    // Detection tracking for audio
    public static bool IsDetected = false;
    public static int DetectionCount = 0;
}
