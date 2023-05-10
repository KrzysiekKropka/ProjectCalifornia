using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Transform
    [SerializeField] private Transform player;

    //Floats
    private float maxScreenPoint = 0.6f; //Position between cursor and player
    private float smoothTime = 0.125f;

    //Vectors
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 mousePos;

    void LateUpdate()
    {
        mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        targetPosition = (player.position + Camera.main.ScreenToWorldPoint(mousePos)) / 2f + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}