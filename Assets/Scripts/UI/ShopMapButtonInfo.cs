using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMapButtonInfo : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    [SerializeField] TMP_Text priceText;
    public int itemID;

    void Start()
    {
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if(priceText!=null)
        {
            if (ShopManager.GetComponent<ShopManager>().shopMaps[3, itemID] >= 1)
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = ShopManager.GetComponent<ShopManager>().shopMaps[2, itemID] + "XP";
            }
        }
    }
}
