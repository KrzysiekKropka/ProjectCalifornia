using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInstantiate : MonoBehaviour
{
    public GameObject settingsPrefab;

    public void InstantiateSettings()
    {
        GameObject settings = Instantiate(settingsPrefab);
        settings.SetActive(true);
    }
}
