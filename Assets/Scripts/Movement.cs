using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float spd;
    public float xAxis;
    private Rigidbody2D rb;

    public float jumpForce;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Movement
        xAxis = Input.GetAxisRaw("Horizontal") * spd;

        //Move with X = Left right
        Vector2 VelocityX = rb.velocity;
        VelocityX.x = xAxis;
        rb.velocity = VelocityX;

        //Max Movement spd
        if (rb.velocity.magnitude > spd)
        {
            rb.velocity *= spd / rb.velocity.magnitude;
        }

        

    }
}
