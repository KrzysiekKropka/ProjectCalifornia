using UnityEngine;

/// <summary>
/// Krzysiek: podjebane z tego filmiku: https://www.youtube.com/watch?v=ZBj3LBA2vUY
/// </summary>

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    float maxScreenPoint = 0.6f;
    float smoothTime = 0.125f;
    Vector3 offset = new Vector3(0f, 0f, -10f);
    Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;
    Vector3 mousePos;

    void LateUpdate()
    {
        mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        targetPosition = (player.position + Camera.main.ScreenToWorldPoint(mousePos)) / 2f + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}