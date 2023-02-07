using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TMP_Text priceText;
    public TMP_Text quantityText;
    public GameObject ShopManager;

    void Update()
    {
        priceText.text = ShopManager.GetComponent<ShopManager>().shopItems[2,itemID].ToString() + "$";
        quantityText.text = ShopManager.GetComponent<ShopManager>().shopItems[3, itemID].ToString() + "X";
    }
}
