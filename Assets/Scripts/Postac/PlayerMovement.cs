using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb;
    public Transform player;

    Vector3 forwardDirection;
    Vector3 smoothedForwardDirection;
    Vector3 forwardDirectionSmoothVelocity;

    Vector3 sideDirection;
    Vector3 smoothedSideDirection;
    Vector3 sideDirectionSmoothVelocity;



    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        forwardDirection = player.transform.rotation * Vector3.up;
        sideDirection = player.transform.rotation * Vector3.left;

        // Krzysiek: wyt�umaczenie tego znajdziesz tu (1:30 minuta): https://www.youtube.com/watch?v=V3NR1n-UhI0
        // Pomaga to w p�ynnym ruchu postaci przez przej�cie od smoothedMovementInput do celu, kt�rym jest movementInput w 0.1 sekundy
        if (Input.GetKey(KeyCode.W))
        {
            smoothedForwardDirection = Vector3.SmoothDamp(smoothedForwardDirection, forwardDirection,
            ref forwardDirectionSmoothVelocity, 0.1f);
            
        }
        rb.velocity = smoothedForwardDirection * speed;

        //smoothedSideDirection = Vector3.SmoothDamp(smoothedSideDirection, sideDirection,
        //    ref sideDirectionSmoothVelocity, 0.1f);


    }
}


/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody2D rb;

    Vector2 movementInput;
    Vector2 smoothedMovementInput;
    Vector2 movementInputSmoothVelocity;

    void Update()
    {
        // Krzysiek: otrzymujemy dane wej�ciowe, np. Horizontal kt�rym b�dzie a i d.
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    { 
        // Krzysiek: wyt�umaczenie tego znajdziesz tu (1:30 minuta): https://www.youtube.com/watch?v=V3NR1n-UhI0
        // Pomaga to w p�ynnym ruchu postaci przez przej�cie od smoothedMovementInput do celu, kt�rym jest movementInput w 0.1 sekundy
        smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput, 
            ref movementInputSmoothVelocity, 0.1f);

        rb.velocity = smoothedMovementInput * speed;
    }
}

 */