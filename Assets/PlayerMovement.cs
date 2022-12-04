using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Rigidbody2D rb;

    Vector2 movementInput;
    Vector2 smoothedMovementInput;
    Vector2 movementInputSmoothVelocity;

    void Update()
    {
        // Krzysiek: otrzymujemy dane wejœciowe, np. Horizontal którym bêdzie a i d.
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {

        // Krzysiek: wyt³umaczenie tego znajdziesz tu (1:30 minuta): https://www.youtube.com/watch?v=V3NR1n-UhI0
        // Pomaga to w p³ynnym ruchu postaci przez przejœcie od smoothedMovementInput do celu, którym jest movementInput w 0.1 sekundy
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, 
            ref movementInputSmoothVelocity, 0.1f);

        rb.velocity = smoothedMovementInput * moveSpeed;
    }
}
