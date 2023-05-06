using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    [SerializeField] AudioSource audioSource;

    public void PlayMap()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID] == 1)
        {
            SceneManager.LoadScene("Lvl" + ButtonRef.GetComponent<ShopMapButtonInfo>().itemID);
        }
    }
}
