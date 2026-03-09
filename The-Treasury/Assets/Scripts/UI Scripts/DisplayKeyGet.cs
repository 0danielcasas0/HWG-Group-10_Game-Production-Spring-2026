using UnityEngine;

public class DisplayKeyGet : MonoBehaviour
{
    public PlayerStats PlayerStats;
    public GameObject KeyGet;

    public float notifDuration = 3f;

    private float timer;

    void Update()
    {
        if (PlayerStats.HasKey)
        {
            KeyGet.SetActive(true);
            timer = notifDuration;
        }

        if (KeyGet.activeSelf)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                KeyGet.SetActive(false);
            }
        }
    }
}
