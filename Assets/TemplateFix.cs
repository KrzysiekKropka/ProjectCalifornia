using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateFix : MonoBehaviour
{
    private void OnEnable()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas)
        {
            print("canvas found");
            canvas.sortingOrder = 2;
        }
    }
}
