using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// KK: TODO: Komentarze moga byc w przyszlosci potrzebne. 
/// </summary>

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject settingsPrefab;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField] AudioSource audioSource;

    public static bool isPaused;
    private GameObject settings;

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
        //inventoryMenu.SetActive(false);
        if (settings != null) Destroy(settings);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void InstantiateSettings()
    {
        settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }

    public void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }
}
