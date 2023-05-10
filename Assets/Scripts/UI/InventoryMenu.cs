using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        HideDuringGameplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !NextLevelScreen.isActive && !PauseMenu.isPaused)
        {
            if (!Player.inInventory)
            {
                ShowDuringGameplay();
            }
            else
            {
                HideDuringGameplay();
            }
        }
    }

    public void ShowDuringGameplay()
    {
        inventoryMenu.SetActive(true);
        Time.timeScale = 0.33f;
        Player.inInventory = true;
    }

    public void HideDuringGameplay()
    {
        inventoryMenu.SetActive(false);
        Time.timeScale = 1f;
        Player.inInventory = false;
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
