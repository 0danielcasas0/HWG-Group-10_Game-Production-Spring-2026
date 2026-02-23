using UnityEngine;

public class QuitManager : MonoBehaviour
{
	public void Quit()
	{
		// save volume values before exiting
		
		Debug.Log("Quit button pressed\nGame exiting...");
		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false;

	}
}

