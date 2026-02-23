using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	// Checks if the user has played the game before,
	// and if not, sets default values for volume.
	// Also saves the user's volume choice for other scenes.

	private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BGMVolumePref = "BGMVolumePref";
    private static readonly string SoundEffectsVolumePref = "SoundEffectsVolumePref";

	private int firstPlayInt;   
    public Slider BGMVolumeSlider, soundEffectsSlider;
    private float BGMVolume;
    private float soundEffectsVolume;

    public AudioSource BGMSource;
    public AudioSource[] SoundEffectsAudio;

	void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            BGMVolume = 0.25f;
            soundEffectsVolume = 0.75f;
			BGMVolumeSlider.value = BGMVolume;
            soundEffectsSlider.value = soundEffectsVolume;

            // Saves the volume and takes the user out of FirstPlay.
			PlayerPrefs.SetFloat("BGMVolumePref", BGMVolume);
            PlayerPrefs.SetFloat("SoundEffectsVolumePref", soundEffectsVolume);
			PlayerPrefs.SetInt(FirstPlay, 1);
        }
        else
        {
			// Will load the saved volume value.
			BGMVolume = PlayerPrefs.GetFloat(BGMVolumePref);
            soundEffectsVolume = PlayerPrefs.GetFloat(SoundEffectsVolumePref);

            BGMVolumeSlider.value = BGMVolume;
            soundEffectsSlider.value = soundEffectsVolume;
		}
	}

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BGMVolumePref, BGMVolumeSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsVolumePref, soundEffectsSlider.value);
        PlayerPrefs.Save();
    }

	private void OnApplicationFocus(bool focus)
	{
		    if (!focus)
        {
            SaveSoundSettings();
		}
	}

    public void UpdateSound()
    {
        BGMSource.volume = BGMVolumeSlider.value;

        for (int i = 0; i < SoundEffectsAudio.Length; i++)
        {
            SoundEffectsAudio[i].volume = soundEffectsSlider.value;
		}
	}

}
