using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;
    public int requiredVisitedScenes;

    public static List<int> visitedScenes = new List<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(visitedScenes.Contains(sceneBuildIndex) || visitedScenes.Count < requiredVisitedScenes){
            return;
        }

        print("Trigger Entered");

        if(other.tag == "Player")
        {
            print("Switching Scene to " + sceneBuildIndex);
            visitedScenes.Add(sceneBuildIndex);
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single); 
        }
    }
}
