using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
	// Public method to load a scene by its name
	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}

