using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Transform enemy;

    private IEnumerator dialogueCoroutine;

    Vector3 offset = new Vector3(0f, 2.375f, 0f);

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Dialogue(string text)
    {
        gameObject.SetActive(true);
        dialogueText.text = text;
        if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = DialogueInterval();
        StartCoroutine(dialogueCoroutine);
    }

    private IEnumerator DialogueInterval()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}