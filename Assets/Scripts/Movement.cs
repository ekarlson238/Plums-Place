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
    private bool dashing;

    [SerializeField]
    private float dashMult;

    private float dashVelocity;

    [SerializeField]
    private float dashDuration;
    private float dashVar;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = true;
        canDash = false;
        dashing = false;
        dashVar = dashDuration;
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

        //press shift to dash if you haven't dashed before, you are not grounded, and you are moving in a horizonal direction
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Input.GetAxisRaw("Horizontal") != 0)
        {
            dashVelocity = xAxis * dashMult;
            
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.AddForce(new Vector2(dashVelocity, 0));
            dashing = true;

            canDash = false; //can only dash once per jump
        }

        if (!dashing)
        {
            rb.velocity = VelocityX; //sets velocity
        }
        else
        {
            dashVar -= Time.deltaTime;
            if (dashVar < 0)
            {
                dashing = false;
                dashVar = dashDuration;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }

    }//fixedUpdate

    

}
