using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsPrefab;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] SettingsMenu settings;

    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        if (PlayerPrefs.HasKey("VolumeValue")) AudioListener.volume = PlayerPrefs.GetFloat("VolumeValue");
        else
        {
            AudioListener.volume = 0.5f;
            PlayerPrefs.SetFloat("VolumeValue", 0.5f);
        };

        if (!PlayerPrefs.HasKey("MusicVolumeValue")) PlayerPrefs.SetFloat("MusicVolumeValue", 0.05f);

        if (!PlayerPrefs.HasKey("postProcessingEnabled"))
        {
            PlayerPrefs.SetInt("postProcessingEnabled", 1);
        }
    }

    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Lvl"+level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
