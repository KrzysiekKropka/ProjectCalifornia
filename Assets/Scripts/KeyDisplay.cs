using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.SetText($"{LevelMove_Ref.visitedScenes.Count}/5");
    }
}
