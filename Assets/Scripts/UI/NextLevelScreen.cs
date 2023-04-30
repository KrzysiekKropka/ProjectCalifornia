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
    int countdown = 5;

    public static bool isActive = false;

    public void StartCountdown(bool isEnd = false)
    {
        isActive = true;
        gameObject.SetActive(true);
        GameObject.FindWithTag("LevelUnlocker").GetComponent<LevelUnlocker>().UnlockLevels();
        StartCoroutine(Countdown(isEnd));
    }

    IEnumerator Countdown(bool isEnd)
    {
        if(!isEnd)
        {
            int currCountdownValue = countdown;
            slider.maxValue = countdown;
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
        else if (isEnd)
        {
            generalText.text = "CONGRATULATIONS!";
            int currCountdownValue = 10;
            slider.maxValue = 10;
            while (currCountdownValue > 0)
            {
                nextLevelCountdownText.text = "Ending in " + currCountdownValue + " seconds!";
                slider.value = currCountdownValue;
                yield return new WaitForSeconds(1);
                currCountdownValue--;
            }
            nextLevelCountdownText.text = "Ending in " + currCountdownValue + " seconds!";
            slider.value = currCountdownValue;
            yield return new WaitForSeconds(1);
            isActive = false;
            SceneManager.LoadScene("Menu");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}