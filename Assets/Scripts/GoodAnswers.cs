using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodAnswers : MonoBehaviour
{
    public void goToBack()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
