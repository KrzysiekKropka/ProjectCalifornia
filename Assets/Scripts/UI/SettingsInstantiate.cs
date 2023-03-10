using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsInstantiate : MonoBehaviour
{
    [SerializeField] GameObject settingsPrefab;

    private GameObject settings;

    public void InstantiateSettings()
    {
        settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }
}
