using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public Text QuestionTxt;
    public GameObject goodAnswerIndicator;
    public GameObject badAnswerIndicator;

    public int requiredCorrectAnswers;
    private int correctAnswers = 0;
    private Coroutine answerIndicatorCoroutine;

    private void Start()
    {
        goodAnswerIndicator.SetActive(false);
        badAnswerIndicator.SetActive(false);
        generateQuestion();
    }

    public void nextScene()
    {
        correctAnswers++;
        if(correctAnswers == 3){
            SceneManager.LoadScene(1);
        } 
    }

    public void correct()
    {
        correctAnswers++;
        QnA.RemoveAt(currentQuestion);
        NextQuestion();
        ResetIndicators();
        answerIndicatorCoroutine = StartCoroutine(ShowGoodAnswerIndicator());
    }

    public void wrong(){
        NextQuestion();
        ResetIndicators();
        answerIndicatorCoroutine = StartCoroutine(ShowBadAnswerIndicator());
    }

    private void ResetIndicators(){
        if(answerIndicatorCoroutine != null){
            StopCoroutine(answerIndicatorCoroutine);
            answerIndicatorCoroutine = null;
        }

        badAnswerIndicator.SetActive(false);
        goodAnswerIndicator.SetActive(false);
    }

    private void NextQuestion(){
        if(QnA.Count == 0){
            SceneManager.LoadScene(1);
        }

        generateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if(QnA[currentQuestion].CorrectAnswer == i+1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        

        currentQuestion = Random.Range(0, QnA.Count);

        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswers();

        
    }
    
    private IEnumerator ShowGoodAnswerIndicator(){
        goodAnswerIndicator.SetActive(true);
        yield return new WaitForSeconds(1f);
        goodAnswerIndicator.SetActive(false);
    }

    private IEnumerator ShowBadAnswerIndicator(){
        badAnswerIndicator.SetActive(true);
        yield return new WaitForSeconds(1f);
        badAnswerIndicator.SetActive(false);
    }
}
