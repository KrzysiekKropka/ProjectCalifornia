using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip brandonPhoneCall;

    void Start()
    {
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void EndCredits()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayBrandon()
    {
        audioSource.clip = brandonPhoneCall;
        audioSource.Play();
    }
}
