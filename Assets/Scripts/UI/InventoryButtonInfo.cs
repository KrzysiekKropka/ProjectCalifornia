using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButtonInfo : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    [SerializeField] GameObject player;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text reloadText;
    Button button;
    bool isOwned;
    public int itemID;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (infoText!=null) AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (ShopManager.GetComponent<ShopManager>().shopItems[3, itemID] >= 1)
        {
            isOwned = true;
            button.interactable = true;
        }
        else
        {
            infoText.text = "Not owned!";
            isOwned = false;
            button.interactable = false;
        }
    }

    void Update()
    {
        if (isOwned)
        {
            infoText.text = player.GetComponent<Shooting>().currentAmmo[itemID] + "/" + player.GetComponent<Shooting>().reserveAmmo[itemID];
            if (player.GetComponent<Shooting>().isReloading[itemID])
            {
                reloadText.text = "Reloading...";
            }
            else
            {
                reloadText.text = "";
            }
        }
    }
}
