using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
  public bool isCorrect = false;
  public QuizManager quizManager;
  public int goodAnswers = 0;
  public GoodAnswers gg;
  
  public void Answer()
  {
    if(isCorrect)
    {
        Debug.Log("Correct Answer");
        quizManager.correct();
        quizManager.nextScene();
        
    }
    else{
        Debug.Log("Wrong Answer");
        quizManager.wrong();
    }
  }
}
