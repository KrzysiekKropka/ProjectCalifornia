using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevelScreen : MonoBehaviour
{
    [SerializeField] TMP_Text nextLevelCountdownText;
    [SerializeField] Slider slider;
    int countdown = 5;

    public static bool isActive = false;

    public void StartCountdown()
    {
        slider.maxValue = countdown;
        isActive = true;
        gameObject.SetActive(true);
        GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().UnlockLevels();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        int currCountdownValue = countdown;
        while (currCountdownValue > 0)
        {
            nextLevelCountdownText.text = "Starting the next level in " + currCountdownValue + " seconds!";
            slider.value = currCountdownValue;
            yield return new WaitForSeconds(1);
            currCountdownValue--;
        }
        nextLevelCountdownText.text = "Starting the next level in " + currCountdownValue + " seconds!";
        slider.value = currCountdownValue;
        yield return new WaitForSeconds(1);
        isActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}