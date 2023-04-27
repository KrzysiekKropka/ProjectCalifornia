using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;

    public int levelID;
    public bool isArena;
    public bool isTheEnd;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip musicClip;
    [SerializeField] int[] ToUnlock;

    void Start()
    {
        audioSource.clip = musicClip;
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolumeValue");
        audioSource.Play();

        if (isArena) GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>().ArenaMode();
    }

    public void UnlockLevels()
    {
        for (int i = 0; i < ToUnlock.Length; i++)
        {
            ShopManager.GetComponent<ShopManager>().shopMaps[3, ToUnlock[i]] = 1;
            PlayerPrefs.SetInt("mapQuantity" + ToUnlock[i], 1);
        }
    }
}
