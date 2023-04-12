using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<Player>().kills>= collision.gameObject.GetComponent<Player>().enemies) collision.gameObject.GetComponent<Player>().TriggerNextLevel();
        }
    }
}
