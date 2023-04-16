using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidescreenNotification : MonoBehaviour
{
    void Start()
    {
        if ((float)Screen.width / (float)Screen.height > (21f/9f)+.1f || (float)Screen.width / (float)Screen.height < (4f / 3f) - .1f)
            GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>().MessageBox("WARNING! This game does not support aspect ratios over 21:9 or under 4:3!");
    }

    void Update()
    {
        
    }
}
