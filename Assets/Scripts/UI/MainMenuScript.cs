using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject settingsPrefab;

    private GameObject settings;

    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Lvl"+level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InstantiateSettings()
    {
        settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }
}
