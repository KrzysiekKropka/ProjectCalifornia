using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsPrefab;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;
    private GameObject settings;

    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        if (PlayerPrefs.HasKey("VolumeValue")) AudioListener.volume = PlayerPrefs.GetFloat("VolumeValue");
        else
        {
            AudioListener.volume = 0.5f;
            PlayerPrefs.SetFloat("VolumeValue", 0.5f);
        };
    }

    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Lvl"+level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InstantiateSettings()
    {
        settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
