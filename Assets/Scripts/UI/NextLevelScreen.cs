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

    public static bool isActive = false;
    int currCountdownValue;

    public void StartCountdown(int seconds)
    {
        slider.maxValue = seconds;
        isActive = true;
        gameObject.SetActive(true);
        GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().UnlockLevels();
        StartCoroutine(Countdown(seconds));
    }

    IEnumerator Countdown(int countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            nextLevelCountdownText.text = "Starting the next level in " + currCountdownValue + " seconds!";
            slider.value = currCountdownValue;
            yield return new WaitForSeconds(1);
            currCountdownValue--;
        }
        isActive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}