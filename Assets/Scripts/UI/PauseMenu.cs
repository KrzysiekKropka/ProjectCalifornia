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
    [SerializeField] GameObject inventoryPrefab;

    bool isPaused;
    private GameObject settings;
    private GameObject inventory;

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
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        healthBar.SetActive(true);
        if (settings != null) Destroy(settings);
        else if (inventory != null) Destroy(inventory);
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
    public void InstantiateInventory()
    {
        inventory = Instantiate(inventoryPrefab);
        inventory.SetActive(true);
    }
}
