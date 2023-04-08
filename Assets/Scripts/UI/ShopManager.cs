using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    //KK: Poradnik na to: https://www.youtube.com/watch?v=Oie-G5xuQNA

    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text xpText;
    [SerializeField] AudioClip moneyClip;
    [SerializeField] AudioSource audioSource;

    public int[,] shopItems = new int[5, 5];
    public int[,] shopSkills = new int[5, 5];
    public int[,] shopMaps = new int[5, 5];

    int money;
    int xp;

    void Start()
    {
        AssignNumbers();

        //KK: ustawianie ID przedmiotom w sklepie
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //KK: ustawianie ID skillom w sklepie
        shopSkills[1, 1] = 1;
        shopSkills[1, 2] = 2;
        shopSkills[1, 3] = 3;
        shopSkills[1, 4] = 4;

        //KK: ustawianie ID mapom w sklepie
        shopMaps[1, 1] = 1;
        shopMaps[1, 2] = 2;
        shopMaps[1, 3] = 3;
        shopMaps[1, 4] = 4;

        //KK: ustawianie ceny przedmiotom w sklepie
        shopItems[2, 1] = 1500;
        shopItems[2, 2] = 3000;
        shopItems[2, 3] = 4500;
        shopItems[2, 4] = 6000;

        //KK: ustawianie ceny skillom w sklepie
        shopSkills[2, 1] = 150;
        shopSkills[2, 2] = 300;
        shopSkills[2, 3] = 450;
        shopSkills[2, 4] = 600;

        //KK: ustawianie ceny mapom w sklepie
        shopMaps[2, 1] = 300;
        shopMaps[2, 2] = 600;
        shopMaps[2, 3] = 900;
        shopMaps[2, 4] = 1200;

        //KK: ustawianie ilosci przedmiotom w sklepie
        shopItems[3, 1] = PlayerPrefs.GetInt("itemQuantity" + 1);
        shopItems[3, 2] = PlayerPrefs.GetInt("itemQuantity" + 2);
        shopItems[3, 3] = PlayerPrefs.GetInt("itemQuantity" + 3);
        shopItems[3, 4] = PlayerPrefs.GetInt("itemQuantity" + 4);

        //KK: ustawianie ilosci skillom w sklepie
        shopSkills[3, 1] = PlayerPrefs.GetInt("skillQuantity" + 1);
        shopSkills[3, 2] = PlayerPrefs.GetInt("skillQuantity" + 2);
        shopSkills[3, 3] = PlayerPrefs.GetInt("skillQuantity" + 3);
        shopSkills[3, 4] = PlayerPrefs.GetInt("skillQuantity" + 4);

        //KK: ustawianie ilosci mapom w sklepie
        shopMaps[3, 0] = 1;
        shopMaps[3, 1] = PlayerPrefs.GetInt("mapQuantity" + 1);
        shopMaps[3, 2] = PlayerPrefs.GetInt("mapQuantity" + 2);
        shopMaps[3, 3] = PlayerPrefs.GetInt("mapQuantity" + 3);
        shopMaps[3, 4] = PlayerPrefs.GetInt("mapQuantity" + 4);
    }

    void ResetQuantity()
    {
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        PlayerPrefs.SetInt("itemQuantity" + 1, 0);
        PlayerPrefs.SetInt("itemQuantity" + 2, 0);
        PlayerPrefs.SetInt("itemQuantity" + 3, 0);
        PlayerPrefs.SetInt("itemQuantity" + 4, 0);
        PlayerPrefs.SetInt("skillQuantity" + 1, 0);
        PlayerPrefs.SetInt("skillQuantity" + 2, 0);
        PlayerPrefs.SetInt("skillQuantity" + 3, 0);
        PlayerPrefs.SetInt("skillQuantity" + 4, 0);
        PlayerPrefs.SetInt("mapQuantity" + 1, 0);
        PlayerPrefs.SetInt("mapQuantity" + 2, 0);
        PlayerPrefs.SetInt("mapQuantity" + 3, 0);
        PlayerPrefs.SetInt("mapQuantity" + 4, 0);
        PlayerPrefs.DeleteKey("equippedWeaponID");
    }

    public void AssignNumbers()
    {
        money = PlayerPrefs.GetInt("money");
        xp = PlayerPrefs.GetInt("experiencePoints");
        if (moneyText != null || xpText != null)
        {
            moneyText.text = "<sprite=0>" + money + "$";
            xpText.text = xp + "<sprite=0>";
        }
    }

    public void BuyItem()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if ((money >= shopItems[2, ButtonRef.GetComponent<ShopItemButtonInfo>().itemID]) && (shopItems[3, ButtonRef.GetComponent<ShopItemButtonInfo>().itemID] < 1))
        {
            money -= shopItems[2, ButtonRef.GetComponent<ShopItemButtonInfo>().itemID];
            shopItems[3, ButtonRef.GetComponent<ShopItemButtonInfo>().itemID]=1;
            moneyText.text = "<sprite=0>" + money + "$";
            PlayerPrefs.SetInt("itemQuantity" + ButtonRef.GetComponent<ShopItemButtonInfo>().itemID, shopItems[3, ButtonRef.GetComponent<ShopItemButtonInfo>().itemID]);
            PlayerPrefs.SetInt("money", money);
            audioSource.PlayOneShot(moneyClip);
        }
    }

    public void BuySkill()
    {
        //kod na kupowanie skillu
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if ((xp >= shopSkills[2, ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID]) && (shopSkills[3, ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID] < 1))
        {
            xp -= shopSkills[2, ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID];
            shopSkills[3, ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID]=1;
            xpText.text = xp + "<sprite=0>";
            PlayerPrefs.SetInt("skillQuantity" + ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID, shopSkills[3, ButtonRef.GetComponent<ShopSkillButtonInfo>().itemID]);
            PlayerPrefs.SetInt("experiencePoints", xp);
            audioSource.PlayOneShot(moneyClip);
        }
    }
}
