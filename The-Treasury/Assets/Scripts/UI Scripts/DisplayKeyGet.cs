using UnityEngine;

public class DisplayKeyGet : MonoBehaviour
{
    public PlayerStats PlayerStats;
    public GameObject KeyGet;

    void Update()
    {
        if (PlayerStats.HasKey == true)
        {
            KeyGet.SetActive(true);
        }
        else
        {
            KeyGet.SetActive(false);
        }
    }
}
