using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;

    public void DestroyItself()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject,1f);
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
