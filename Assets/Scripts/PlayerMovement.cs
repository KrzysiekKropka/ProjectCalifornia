using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< HEAD
    public float moveSpeed;
    public Rigidbody2D rb;

    private Animator anim;

    private Vector2 moveDirection;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    void Update()
    {
        ProcessInputs();
    }


    void FixedUpdate()
    {
        Move();
        
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY);

        if (moveX > 0)
        {
            anim.SetInteger("FazaAnimacji",1);
            
        }
        else if (moveX == 0)
        {
            anim.SetInteger("FazaAnimacji",0);
        }
        if (moveX < 0)
        {
            anim.SetInteger("FazaAnimacji",2);
        }
        
        
        
        print(moveX);
       
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

=======
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
>>>>>>> 980f41b45faf854ab6bd276931788689f3ec1c4b
}
