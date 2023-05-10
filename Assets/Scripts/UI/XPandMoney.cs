using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPandMoney : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text experiencePointsText;

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
