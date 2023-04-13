using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TMP_Text equippedText;
    private GameObject player;

    int equippedWeaponID;
    string equippedWeaponName;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
        equippedWeaponName = PlayerPrefs.GetString("equippedWeaponName");
        AssignNames();
    }

    public void EquipItem()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (PlayerPrefs.GetInt("itemQuantity" + ButtonRef.GetComponent<InventoryButtonInfo>().itemID) == 1)
        {
            equippedWeaponID = ButtonRef.GetComponent<InventoryButtonInfo>().itemID;
            PlayerPrefs.SetInt("equippedWeaponID", equippedWeaponID);
            if (player!=null)
            {
                player.GetComponent<Shooting>().AssignWeapon(equippedWeaponID);
            }
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
