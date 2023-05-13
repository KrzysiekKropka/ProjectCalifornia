using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevelScreen : MonoBehaviour
{
    [SerializeField] TMP_Text nextLevelCountdownText;
    [SerializeField] TMP_Text generalText;
    [SerializeField] Slider slider;

    private IEnumerator countdownCoroutine;

    public static bool isActive = false;

    public void StartCountdown()
    {
        isActive = true;
        gameObject.SetActive(true);
        GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().UnlockLevels();
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
        countdownCoroutine = Countdown();
        StartCoroutine(countdownCoroutine);
    }

    IEnumerator Countdown()
    {
        int currCountdownValue = 5;
        slider.maxValue = 5;
        while (currCountdownValue > 0)
        {
            nextLevelCountdownText.text = "Starting the next level in " + currCountdownValue + " seconds!";
            slider.value = currCountdownValue;
            yield return new WaitForSeconds(1);
            currCountdownValue--;
        }
        nextLevelCountdownText.text = "Starting the next level in " + currCountdownValue + " seconds!";
        slider.value = currCountdownValue;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        yield return new WaitForSeconds(1);
        isActive = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}