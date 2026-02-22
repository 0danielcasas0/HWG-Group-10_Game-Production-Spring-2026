using UnityEngine;

public class DataManager : MonoBehaviour
{
	private int volumeValue;
	public int VolumeValue { get => volumeValue;  set => volumeValue = value; }	

	public static DataManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

	}
}
