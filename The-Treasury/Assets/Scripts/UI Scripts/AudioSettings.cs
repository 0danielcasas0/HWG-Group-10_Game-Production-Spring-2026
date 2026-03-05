using UnityEngine;

public class AudioSettings : MonoBehaviour
{
	private static readonly string BGMVolumePref = "BGMVolumePref";
	private static readonly string SoundEffectsVolumePref = "SoundEffectsVolumePref";
	private float BGMVolume;
	private float soundEffectsVolume;
	public AudioSource BGMSource;
	public AudioSource BGMChaseSource;
    public AudioSource[] soundEffectsAudio;
	public AudioSource TrainInteriorSFX;
	public AudioSource FootstepsSFX;

    void Awake()
	{
		ContinueSettings();
		TrainInteriorSFX.volume = soundEffectsVolume * .5f;
		FootstepsSFX.volume = soundEffectsVolume * .5f;
    }

	private void ContinueSettings()
	{
        // Will load the saved volume value.
        BGMVolume = PlayerPrefs.GetFloat(BGMVolumePref);
        soundEffectsVolume = PlayerPrefs.GetFloat(SoundEffectsVolumePref);

        // Set BGM Audiosource.
        BGMSource.volume = BGMVolume;
		BGMChaseSource.volume = BGMVolume;

        // Lets you put a number for the amount of sound effects in the
        // scene that it will control the volume of.
        for (int i = 0; i < soundEffectsAudio.Length; i++)
		{
			soundEffectsAudio[i].volume = soundEffectsVolume;
		}
	}
}
