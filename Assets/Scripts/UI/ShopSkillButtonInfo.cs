using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSkillButtonInfo : MonoBehaviour
{
    [SerializeField] TMP_Text quantityText;
    [SerializeField] GameObject ShopManager;

    public int itemID;

    void Start()
    {
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (quantityText != null)
        {
            if (ShopManager.GetComponent<ShopManager>().shopSkills[3, itemID] >= 1)
            {
                quantityText.text = ShopManager.GetComponent<ShopManager>().shopSkills[3, itemID].ToString() + "x\n" + ShopManager.GetComponent<ShopManager>().shopSkills[2, itemID].ToString() + "XP";
            }
            else
            {
                quantityText.text = ShopManager.GetComponent<ShopManager>().shopSkills[2, itemID].ToString() + "XP";
            }
        }
    }
}
