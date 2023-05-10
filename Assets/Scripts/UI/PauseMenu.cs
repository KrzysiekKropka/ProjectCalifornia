using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] GameObject shop;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;

    public static bool isPaused;

    void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !NextLevelScreen.isActive)
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        healthBar.SetActive(false);
        inventory.GetComponent<InventoryMenu>().HideDuringGameplay();
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        healthBar.SetActive(true);
        inventoryMenu.SetActive(false);
        shop.SetActive(false);
        settings.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
