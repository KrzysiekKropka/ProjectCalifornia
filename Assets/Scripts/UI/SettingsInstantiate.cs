using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsInstantiate : MonoBehaviour
{
    public GameObject settingsPrefab;
    GameObject settings;
    Scene currentScene;

    public void InstantiateSettings()
    {
        currentScene = SceneManager.GetActiveScene();
        settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }

    void Update()
    {
        if (currentScene.name != "Menu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(settings);
            }
        }
    }
}
