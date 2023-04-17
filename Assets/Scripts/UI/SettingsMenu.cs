using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float globalVolume;
    private float musicVolume;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Awake()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            //string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + "p";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        globalVolume = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = globalVolume;

        musicVolume = PlayerPrefs.GetFloat("MusicVolumeValue");
        musicVolumeSlider.value = musicVolume;

        if (!Screen.fullScreen)
        {
            fullscreenToggle.isOn = false;
            resolutionDropdown.gameObject.SetActive(false);
            Screen.fullScreen = false;
        }
        else
        {
            fullscreenToggle.isOn = true;
            resolutionDropdown.gameObject.SetActive(true);
            Screen.fullScreen = true;
        }
    }

    public void SetFullscreen()
    {
        if(fullscreenToggle.isOn)
        {
            Resolution resolution = filteredResolutions[filteredResolutions.Count - 1];
            resolutionDropdown.value = filteredResolutions.Count - 1;
            Screen.SetResolution(resolution.width, resolution.height, true);
            resolutionDropdown.gameObject.SetActive(true);
        }
        else if(!fullscreenToggle.isOn && Screen.fullScreen)
        {
            Screen.SetResolution(640, 480, false);
            resolutionDropdown.gameObject.SetActive(false);
        }
        else
        {
            Screen.fullScreen = false;
            resolutionDropdown.gameObject.SetActive(false);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void DestroyItself()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject,1f);
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }

    public void VolumeSlider()
    {
        globalVolume = volumeSlider.value;
        AudioListener.volume = globalVolume;
        PlayerPrefs.SetFloat("VolumeValue", globalVolume);
    }

    public void MusicSlider()
    {
        musicVolume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolumeValue", musicVolume);
    }
}
