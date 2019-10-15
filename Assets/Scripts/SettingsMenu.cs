using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    // TiltFactor Slider
    [SerializeField]
    private UnityEngine.UI.Slider tiltFactorSlider;

    // TiltFactor text
    [SerializeField]
    private UnityEngine.UI.Text tiltFactorText;

    // TiltFactor
    public static float tiltFactor = 0.15f;

    // Music Volume slider
    [SerializeField]
    private UnityEngine.UI.Slider musicVolumeSlider;

    // Music volume text
    [SerializeField]
    private UnityEngine.UI.Text musicVolumeText;

    // MusicVolume
    public static float musicVolume = 0.5f;

    // Music player object audio source
    [SerializeField]
    private AudioSource musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("tiltFactor"))
        {
            tiltFactor = PlayerPrefs.GetFloat("tiltFactor");
        }
        else
        {
            PlayerPrefs.SetFloat("tiltFactor", 0.15f);
        }

        tiltFactorSlider.value = tiltFactor;
        tiltFactorText.text = "Tilt Factor: " + tiltFactorSlider.value.ToString("0.00");

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
        }

        musicVolumeSlider.value = musicVolume;
        musicVolumeText.text = "Music Volume: " + musicVolumeSlider.value.ToString("0.00");
        musicPlayer.volume = musicVolume;
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if(tiltFactorSlider.value != tiltFactor)
        {
            tiltFactor = tiltFactorSlider.value;
            tiltFactorText.text = "Tilt Factor: " + tiltFactorSlider.value.ToString("0.00");
            PlayerPrefs.SetFloat("tiltFactor", tiltFactor);
            PlayerPrefs.Save();
        }

        if(musicVolumeSlider.value != musicVolume)
        {
            musicVolume = musicVolumeSlider.value;
            musicVolumeText.text = "Music Volume: " + musicVolumeSlider.value.ToString("0.00");
            musicPlayer.volume = musicVolume;
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.Save();
        }
    }
}
