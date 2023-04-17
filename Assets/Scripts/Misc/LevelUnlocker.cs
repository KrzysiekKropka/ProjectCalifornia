using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;

    public int levelID;
    public float musicVolume;
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] int[] ToUnlock;

    void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolumeValue");
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
