using JetBrains.Annotations;
using UnityEngine;

public class WinLose : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private MonoBehaviour Enemy;

    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;

    private PlayerMovement PlayerMovement;
    private PlayerStats playerStats;

    private bool gameEnded;
    void Awake()
    {
        Time.timeScale = 1f;

        PlayerMovement = Player.GetComponent<PlayerMovement>();
        playerStats = Player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (gameEnded) return;
        if (playerStats.HasGold == true)
        {
            WinLevel();
        }
        else if (playerStats.IsCaught == true)
        {
            LoseLevel();
        }
    }
    public void WinLevel()
    {
        // Makes the WinScreen appear and stops playerMovement, and unlocks the cursor.
        gameEnded = true;
        WinScreen.SetActive(true);
        EndGame();
    }
    public void LoseLevel()
    {
        gameEnded = true;
        LoseScreen.SetActive(true);
        EndGame();
    }

    private void EndGame()
    {
        // Stop player movement.
        if (PlayerMovement != null)
            PlayerMovement.enabled = false;

        // Stop enemy.
        if (Enemy != null)
            Enemy.enabled = false;

        // Unlock cursor.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freezes the game when it ends.
        Time.timeScale = 0f;
    }
}
