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
    [SerializeField] TMP_Text xpText;
    [SerializeField] AudioClip moneyClip;
    [SerializeField] AudioSource audioSource;

    int xp;

    void Start()
    {
        AssignNumbers();
    }

    public void AssignNumbers()
    {
        xp = PlayerPrefs.GetInt("experiencePoints");
        if (xpText != null)
        {
            xpText.text = xp + "<sprite=0>";
        }
    }

    public void PlayMap()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID] >= 1)
        {
            SceneManager.LoadScene("Lvl" + ButtonRef.GetComponent<ShopMapButtonInfo>().itemID);
        }
    }

    /*public void BuyMap()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID] < 1 && xp >= ShopManager.GetComponent<ShopManager>().shopMaps[2, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID])
        {
            xp -= ShopManager.GetComponent<ShopManager>().shopMaps[2, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID];
            ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID] = 1;
            xpText.text = xp + "<sprite=0>";
            PlayerPrefs.SetInt("mapQuantity" + ButtonRef.GetComponent<ShopMapButtonInfo>().itemID, ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID]);
            PlayerPrefs.SetInt("experiencePoints", xp);
            audioSource.PlayOneShot(moneyClip);
        }
        else if (ShopManager.GetComponent<ShopManager>().shopMaps[3, ButtonRef.GetComponent<ShopMapButtonInfo>().itemID] >= 1)
        {
            SceneManager.LoadScene("Lvl" + ButtonRef.GetComponent<ShopMapButtonInfo>().itemID);
        }
    }*/
}
