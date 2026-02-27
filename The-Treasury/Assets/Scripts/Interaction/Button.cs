using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour, IInteractable
{

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Interact()
    {
        LoadSceneByName("Main Menu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
