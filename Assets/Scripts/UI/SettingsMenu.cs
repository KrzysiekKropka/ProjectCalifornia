using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;
    private float globalVolume;

    void Start()
    {
        globalVolume = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = globalVolume;
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
}
