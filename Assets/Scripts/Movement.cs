using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private RigidbodyConstraints2D originalConstraints;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject playerSprite;
    private Vector3 originalScale;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = true;
        canDash = false;
        dashing = false;
        dashVar = dashDuration;
        originalConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        originalScale = playerSprite.transform.localScale;
    }
    

    //if the player collides with something tagged ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true; //let me jump again
            canDash = false; //can't dash if grounded

            animator.SetBool("isJumping", false);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "dashThrough" && !dashing) //if hits laser without dashing
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.tag == "Water") //if touches water
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.tag == "Coin") //if touches coin
        {

        }

    }

    void FixedUpdate()
    {
        this.transform.eulerAngles = new Vector3(0, 0, 0);

        //value for x velocity
        xAxis = Input.GetAxisRaw("Horizontal") * spd;

        //set object's x velocity to xAxis
        Vector2 VelocityX = rb.velocity;
        VelocityX.x = xAxis;
        
        //jump
        if (Input.GetButton("Jump") && grounded)
        {
            VelocityX.y = jumpHeight; //sets y velocity
            grounded = false;//test to see if already jumping
            canDash = true;//can only dash while jumping

            animator.SetBool("isJumping", true);
        }

        //press shift to dash if you haven't dashed before, you are not grounded, and you are moving in a horizonal direction
        if (Input.GetButton("Fire3") && canDash && Input.GetAxisRaw("Horizontal") != 0)
        {
            dashVelocity = xAxis * dashMult;

            freezeY(); //freeze y position while dashing
            rb.AddForce(new Vector2(dashVelocity, 0));
            dashing = true;

            canDash = false; //can only dash once per jump

            animator.SetBool("isDashing", true);
        }

        if (!dashing)//if you're not dashing
        {
            rb.velocity = VelocityX; //sets velocity
            animator.SetFloat("Speed", Mathf.Abs(VelocityX.x));

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                playerSprite.transform.localScale = new Vector3(-(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                playerSprite.transform.localScale = originalScale;
            }

        }
        else//if you are dashing, continue to dash for dashDuration, then stop dashing
        {
            dashVar -= Time.deltaTime;
            if (dashVar < 0)
            {
                dashing = false;
                dashVar = dashDuration;
                unfreezeY();

                animator.SetBool("isDashing", false);
            }
        }

    }//fixedUpdate

    void freezeY()
    {
        rb.constraints = originalConstraints;
    }

    void unfreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


}
