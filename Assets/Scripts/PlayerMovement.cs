using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

}
