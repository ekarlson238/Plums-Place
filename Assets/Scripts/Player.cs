using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField][Tooltip("How fast the player can move")]
    private float speed;
    [SerializeField][Tooltip("How high the player can jump")]
    private float jumpHeight;
    [SerializeField][Tooltip("The number you multiply the base speed by to set the speed of a dash")]
    private float dashsSpeedMultiplier;
    [SerializeField][Tooltip("The duration of the dash")]
    private float dashDurationSave;
    [SerializeField][Tooltip("The player's animator")]
    private Animator animator;
    [SerializeField][Tooltip("The Player's sprite")]
    private GameObject playerSprite;
    [SerializeField][Tooltip("The circle collider under the player that checks ifGrounded")]
    private Collider2D groundDetectTrigger;
    [SerializeField]
    private ContactFilter2D groundContactFilter;
    [SerializeField][Tooltip("The player's shield Game Object")]
    private GameObject shield;
    [SerializeField][Tooltip("The button pressed when dead to respawn")]
    private Button deathButton;
    #endregion

    #region Private Fields
    private float speedSave; //saves the orignal base speed value so I can go back to it
    private float halfSpeedSave; //saves the value of half the base speed so I can use it later
    private float xAxis;
    private Rigidbody2D rb;
    private bool grounded;
    private bool canDash;
    private bool dashing;
    private float dashVelocity;
    private float dashDuration;
    private Vector3 originalScale;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private Vector2 velocity;
    private float horizontalRawAxisValueSave;  //this saves the last GetAxisRaw to represent the direction the player is facing
    private Checkpoint currentCheckpoint;
    #endregion

    void Start()
    {
        Time.timeScale = 1;
        rb = gameObject.GetComponent<Rigidbody2D>();
        grounded = true;
        canDash = false;
        dashing = false;
        dashDuration = dashDurationSave;
        originalScale = playerSprite.transform.localScale;
        velocity = rb.velocity;
        speedSave = speed;
        halfSpeedSave = speed / 2;
        horizontalRawAxisValueSave = 1; //the player starts facing to the right
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "dashThrough" && !dashing) //if hits laser without dashing
        {
            Death();
        }

        if (collision.gameObject.tag == "Water") //if touches water
        {
            Death();
        }
        
    }

    private void Death()
    {
        deathButton.onClick.AddListener(DeathButtonPressed);
        deathButton.gameObject.SetActive(true);
        Time.timeScale = 0;
        
    }

    private void DeathButtonPressed()
    {
        deathButton.gameObject.SetActive(false);

        if (currentCheckpoint == null)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Respawn();
        }

        Time.timeScale = 1;

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
            speed = halfSpeedSave;
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
            horizontalRawAxisValueSave = Input.GetAxisRaw("Horizontal");
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerSprite.transform.localScale = originalScale;
            horizontalRawAxisValueSave = Input.GetAxisRaw("Horizontal");
        }
    }

    private void Dash()
    {
        //press shift to dash if you haven't dashed before, you are not grounded, and you are moving in a horizonal direction
        if (Input.GetButton("Fire3") && canDash && !grounded)
        {
            dashVelocity = horizontalRawAxisValueSave * speedSave * dashsSpeedMultiplier;

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

    private void FreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation| RigidbodyConstraints2D.FreezePositionY;
    }

    private void UnfreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetCurrentCheckpoint(Checkpoint newCurrentCheckpoint)
    {
        currentCheckpoint = newCurrentCheckpoint;
    }

    private void Respawn()
    {
        rb.velocity = new Vector2(0, 0);
        transform.position = currentCheckpoint.transform.position;
    }


}
