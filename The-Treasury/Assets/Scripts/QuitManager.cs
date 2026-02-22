using UnityEngine;

public class QuitManager : MonoBehaviour
{
	public void Quit()
	{
		Debug.Log("Quit button pressed\nGame exiting...");
		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false;

	}
}

