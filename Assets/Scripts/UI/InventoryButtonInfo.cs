using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButtonInfo : MonoBehaviour
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text reloadText;
    [SerializeField] Shooting shooting;
    private Button button;
    public int itemID;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    void OnEnable()
    {
        AssignOwnership();
    }

    public void AssignOwnership()
    {
        if (infoText != null)
        {
            if (PlayerPrefs.GetInt("itemQuantity" + itemID) == 1)
            {
                infoText.text = shooting.currentAmmo[itemID] + "/" + shooting.reserveAmmo[itemID];
                button.interactable = true;
            }
            else
            {
                infoText.text = "Not owned!";
                button.interactable = false;
            }
        }
    }

    /*void LateUpdate()
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
    }*/
}
