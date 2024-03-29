using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius = 10f;
    [Range(1,360)][SerializeField] float angle = 90f;

    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstructionLayer;

    GameObject player;
    AIBrain brain;
    public bool CanSeePlayer { get; private set; }  

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        brain = gameObject.GetComponent<AIBrain>();
        StartCoroutine(FOVCheck());
    }

    void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if(rangeCheck.Length > 0 ) 
        { 
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if(Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if(!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer)) 
                {
                    brain.PlayerInterrupts();
                    CanSeePlayer = true;
                }
                else CanSeePlayer = false;
            }
            else CanSeePlayer = false;
        }
        else if(CanSeePlayer) CanSeePlayer = false;
    }

    IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            FOV();
        }
    }
}
