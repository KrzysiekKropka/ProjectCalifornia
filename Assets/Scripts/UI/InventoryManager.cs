using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    private GameObject player;
    private int equippedWeaponID;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        equippedWeaponID = PlayerPrefs.GetInt("equippedWeaponID");
    }

    public void EquipItem()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (PlayerPrefs.GetInt("itemQuantity" + ButtonRef.GetComponent<InventoryButtonInfo>().itemID) == 1)
            if (player!=null) player.GetComponent<Shooting>().AssignWeapon(ButtonRef.GetComponent<InventoryButtonInfo>().itemID);
    }
}
