using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] GameObject ShopManager;
    [SerializeField] TMP_Text equippedText;

    int equippedWeaponID;
    string equippedWeaponName;

    void Start()
    {
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        equippedWeaponName = PlayerPrefs.GetString("equippedWeaponName");
        AssignNames();
    }

    public void EquipItem()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (ShopManager.GetComponent<ShopManager>().shopItems[3, ButtonRef.GetComponent<InventoryButtonInfo>().itemID] >= 1)
        {
            equippedWeaponID = ButtonRef.GetComponent<InventoryButtonInfo>().itemID;
            PlayerPrefs.SetInt("equippedWeaponID", equippedWeaponID);
            AssignNames();
        }
        else
        {
            equippedText.text = "You don't own this weapon!";
        }
    }

    public void AssignNames()
    {
        switch (equippedWeaponID)
        {
            case 1:
                equippedWeaponName = "Deagle";
                break;
            case 2:
                equippedWeaponName = "MP5";
                break;
            case 3:
                equippedWeaponName = "Shotgun";
                break;
            case 4:
                equippedWeaponName = "AK-47";
                break;
            default:
                equippedWeaponName = "Pistol";
                break;
        }
        PlayerPrefs.SetString("equippedWeaponName", equippedWeaponName);
        equippedText.text = "Equipped " + equippedWeaponName + "!";
    }
}
