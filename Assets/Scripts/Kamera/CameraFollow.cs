using UnityEngine;

/// <summary>
/// Krzysiek: podjebane z tego filmiku: https://www.youtube.com/watch?v=ZBj3LBA2vUY
/// </summary>

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    public Transform player;

    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}