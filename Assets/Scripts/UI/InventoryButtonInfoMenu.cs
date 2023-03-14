using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButtonInfoMenu : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    [SerializeField] TMP_Text infoText;
    public Button button;
    public int itemID;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (ShopManager.GetComponent<ShopManager>().shopItems[3, itemID] >= 1)
        {
            button.interactable = true;
            //infoText.text = "OWNED!";
        }
        else
        {
            button.interactable = false;
        }
    }
}
