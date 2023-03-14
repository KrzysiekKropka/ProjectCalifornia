using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonInfo : MonoBehaviour
{
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text quantityText;
    [SerializeField] GameObject ShopManager;

    public int itemID;

    void Start()
    {
        priceText.text = ShopManager.GetComponent<ShopManager>().shopItems[2,itemID].ToString() + "$";
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (ShopManager.GetComponent<ShopManager>().shopItems[3, itemID] >= 1)
        {
            quantityText.text = "OWNED!";
        }
        else
        {
            quantityText.text = "BUY NOW!";
        }
    }
}
