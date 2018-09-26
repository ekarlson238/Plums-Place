using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField]
    private float spd;

    private float xAxis;
    private Rigidbody2D rb;

    [SerializeField]
    private float jumpHeight;

    private bool grounded;
    private bool canDash;

    [SerializeField]
    private float dashMult;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = true;
        canDash = false;
    }

    //if the player collides with something tagged ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true; //let me jump again
            canDash = false; //can't dash if grounded
        }
    }

    void FixedUpdate()
    {
        //value for x velocity
        xAxis = Input.GetAxisRaw("Horizontal") * spd;

        //set object's x velocity to xAxis
        Vector2 VelocityX = rb.velocity;
        VelocityX.x = xAxis;
        
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            VelocityX.y = jumpHeight; //sets y velocity
            grounded = false;//test to see if already jumping
            canDash = true;//can only dash while jumping
        }

        //press shift to dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            VelocityX.x = xAxis * dashMult;

            canDash = false; //can only dash once per jump
        }

        rb.velocity = VelocityX; //sets velocity

    }//fixedUpdate

    

}
