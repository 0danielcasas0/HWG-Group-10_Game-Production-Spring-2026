using UnityEngine;

public class DisplayKeyGet : MonoBehaviour
{
    public PlayerStats PlayerStats;
    public GameObject KeyGet;

    public float notifDuration = 3f;

    private bool notifTriggered;
    private float timer;

    void Update()
    {
        if (PlayerStats.HasKey && !notifTriggered)
        {
            KeyGet.SetActive(true);
            notifTriggered = true;
            timer = notifDuration;
        }

        if (notifTriggered && KeyGet.activeSelf)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                KeyGet.SetActive(false);
            }
        }
    }
}
