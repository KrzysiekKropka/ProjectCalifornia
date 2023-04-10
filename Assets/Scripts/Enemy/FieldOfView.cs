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
    public bool CanSeePlayer { get; private set; }  

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                    gameObject.GetComponent<AIBrain>().PlayerInterrupts();
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
            yield return new WaitForSeconds(0.05f);
            FOV();
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }*/
}
