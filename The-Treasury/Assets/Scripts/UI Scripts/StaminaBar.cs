using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public GameObject UIContainer;
    public Slider staminaBar;
    public PlayerMovement playerStamina;

    public float hideDelay = 3f;
    public float staminaFullTimer;

    void Start()
    {
        if (playerStamina != null && staminaBar != null)
        {
            staminaBar.maxValue = playerStamina.MaxStamina;
            staminaBar.value = playerStamina.CurrentStamina;
        }

        if (staminaBar.value == playerStamina.MaxStamina)
        {
            UIContainer.SetActive(true);
        }
    }

    private void Update()
    {
        // Updates Stamina bar continuously
        if (playerStamina == null || staminaBar == null)
        {
            return;
        }

        staminaBar.value = playerStamina.CurrentStamina;

        if (playerStamina.CurrentStamina >= playerStamina.MaxStamina)
        {
            // Timer should start when stamina is full
            staminaFullTimer += Time.deltaTime;

            // If full long enough, hide it
            if (staminaFullTimer >= hideDelay)
            {
                UIContainer.SetActive(false);
            }
        }
        else
        {
            staminaFullTimer = 0f;
            UIContainer.SetActive(true);
        }
    }
}
