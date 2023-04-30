using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMapButtonInfo : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    public int itemID;
    Button button;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (ShopManager.GetComponent<ShopManager>().shopMaps[3, itemID] == 1)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
