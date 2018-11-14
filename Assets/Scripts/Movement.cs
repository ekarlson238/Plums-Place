using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

    [SerializeField]
    private float speed;
    private float speedSave;
    private float halfSpeed;

    private float xAxis;
    private Rigidbody2D rb;

    [SerializeField]
    private float jumpHeight;

    private bool grounded;
    private bool canDash;
    private bool dashing;

    [SerializeField]
    private float dashsSpeedMultiplier;

    private float dashVelocity;

    [SerializeField]
    private float dashDurationSave;
    private float dashDuration;
    
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject playerSprite;
    private Vector3 originalScale;

    [SerializeField]
    private Collider2D groundDetectTrigger;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];

    [SerializeField]
    private ContactFilter2D groundContactFilter;

    private Vector2 velocity;

    private float horizontalRawAxisValue;

    [SerializeField]
    private GameObject shield;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = true;
        canDash = false;
        dashing = false;
        dashDuration = dashDurationSave;
        originalScale = playerSprite.transform.localScale;

        velocity = rb.velocity;
        speedSave = speed;
        halfSpeed = speed / 2;

        horizontalRawAxisValue = 1;
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
        
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        SetHorizontalVelocity();

        Jump();

        Dash();

        DashShield();

        MovementDependingOnIfDashing();

    }//fixedUpdate

    private void DashShield()
    {
        if (dashing)
        {
            shield.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            shield.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void UpdateGrounded()
    {
        grounded = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;
        //Debug.Log("grounded =" + grounded);

        if (grounded)
        {
            canDash = true; //landing on ground resets dash

            animator.SetBool("isJumping", false);
            speed = speedSave;
        }
        else
        {
            speed = halfSpeed;
            animator.SetBool("isJumping", true);
        }
    }

    private void MovementDependingOnIfDashing()
    {
        if (!dashing)//if you're not dashing
        {
            rb.velocity = velocity; //sets velocity
            animator.SetFloat("Speed", Mathf.Abs(velocity.x));

            MirrorPlayerSprite();

        }
        else//if you are dashing, continue to dash for dashDuration, then stop dashing
        {
            dashDuration -= Time.deltaTime;
            if (dashDuration < 0)
            {
                dashing = false;
                dashDuration = dashDurationSave;
                UnfreezeY();

                animator.SetBool("isDashing", false);
            }
        }
    }

    private void MirrorPlayerSprite()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            playerSprite.transform.localScale = new Vector3(-(originalScale.x), originalScale.y, originalScale.z);
            horizontalRawAxisValue = Input.GetAxisRaw("Horizontal");
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerSprite.transform.localScale = originalScale;
            horizontalRawAxisValue = Input.GetAxisRaw("Horizontal");
        }
    }

    private void Dash()
    {
        //press shift to dash if you haven't dashed before, you are not grounded, and you are moving in a horizonal direction
        if (Input.GetButton("Fire3") && canDash && !grounded)
        {
            dashVelocity = horizontalRawAxisValue * speedSave * dashsSpeedMultiplier;

            FreezeY(); //freeze y position while dashing
            rb.AddForce(new Vector2(dashVelocity, 0));
            dashing = true;

            canDash = false; //can only dash once per jump

            animator.SetBool("isDashing", true);
        }
    }

    private void Jump()
    {
        //jump
        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpHeight; //sets y velocity
        }
    }

    private void SetHorizontalVelocity()
    {
        //value for x velocity
        xAxis = Input.GetAxisRaw("Horizontal") * speed;

        //set object's x velocity to xAxis
        velocity = rb.velocity;
        velocity.x = xAxis;
    }

    void FreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation| RigidbodyConstraints2D.FreezePositionY;
    }

    void UnfreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


}
