using UnityEngine;

public class AudioSettings : MonoBehaviour
{
	private static readonly string BGMVolumePref = "BGMVolumePref";
	private static readonly string SoundEffectsVolumePref = "SoundEffectsVolumePref";
	private float BGMVolume;
	private float soundEffectsVolume;
	public AudioSource BGMSource;
	public AudioSource[] SoundEffectsAudio;

	void Awake()
	{
		ContinueSettings();
	}

	private void ContinueSettings()
	{
		BGMVolume = PlayerPrefs.GetFloat(BGMVolumePref);
		soundEffectsVolume = PlayerPrefs.GetFloat(SoundEffectsVolumePref);

		// Set BGM Audiosource.
		BGMSource.volume = BGMVolume;

		// Lets you put a number for the amount of sound effects in the
		// scene that it will control the volume of.
		for (int i = 0; i < SoundEffectsAudio.Length; i++)
		{
			SoundEffectsAudio[i].volume = soundEffectsVolume;
		}
	}
}
