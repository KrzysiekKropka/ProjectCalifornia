using UnityEngine;

/// <summary>
/// Krzysiek: podjebane z tego filmiku: https://www.youtube.com/watch?v=ZBj3LBA2vUY
/// </summary>

public class CameraFollow : MonoBehaviour
{
    Vector3 offset = new Vector3(0f, 0f, -10f);
    float smoothTime = 0.075f;
    Vector3 velocity = Vector3.zero;

    public Transform player;

    void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}