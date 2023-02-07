using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    //KK: Poradnik na to: https://www.youtube.com/watch?v=Oie-G5xuQNA

    public int[,] shopItems = new int[5, 5];
    int money;
    public TMP_Text moneyText;

    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        moneyText.text = "Money: " + money + "$";
        //KK: ustawianie ID przedmiotom w sklepie
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //KK: ustawianie ceny przedmiotom w sklepie
        shopItems[2, 1] = 1000;
        shopItems[2, 2] = 2000;
        shopItems[2, 3] = 3000;
        shopItems[2, 4] = 4000;

        //KK: ustawianie ilosci przedmiotom w sklepie
        shopItems[3, 1] = PlayerPrefs.GetInt("itemQuantity" + 1);
        shopItems[3, 2] = PlayerPrefs.GetInt("itemQuantity" + 2);
        shopItems[3, 3] = PlayerPrefs.GetInt("itemQuantity" + 3);
        shopItems[3, 4] = PlayerPrefs.GetInt("itemQuantity" + 4);
    }

    public void BuyItem()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if (money >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID])
        {
            money -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;
            moneyText.text = "Money: " + money + "$";
            ButtonRef.GetComponent<ButtonInfo>().quantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();
            PlayerPrefs.SetInt("itemQuantity" + ButtonRef.GetComponent<ButtonInfo>().itemID, shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]);
            PlayerPrefs.SetInt("money", money);
        }
    }
}
