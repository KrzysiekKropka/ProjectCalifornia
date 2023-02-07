using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPandMoney : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text experiencePointsText;

    int experiencePoints;
    int money;

    void Start()
    {
        experiencePoints = PlayerPrefs.GetInt("experiencePoints");
        money = PlayerPrefs.GetInt("money");
        moneyText.text = "Money: " + money + "$";
        experiencePointsText.text = "XP: " + experiencePoints;
    }
}
