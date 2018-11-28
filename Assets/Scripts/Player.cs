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
    private bool isGrounded;
    private bool canDash;
    private bool isDashing;
    private float dashVelocity;
    private float dashDuration;
    private Vector3 originalScale;
    private Collider2D[] groundHitDetectionResults = new Collider2D[16];
    private Vector2 velocity;
    private float horizontalRawAxisValueSave;  //this saves the last GetAxisRaw to represent the direction the player is facing
    private Checkpoint currentCheckpoint;
    private SpriteRenderer shieldSprite;
    #endregion

    private void Start()
    {
        Time.timeScale = 1;
        rb = gameObject.GetComponent<Rigidbody2D>();
        isGrounded = true;
        canDash = false;
        isDashing = false;
        dashDuration = dashDurationSave;
        originalScale = playerSprite.transform.localScale;
        velocity = rb.velocity;
        speedSave = speed;
        halfSpeedSave = speed / 2;
        horizontalRawAxisValueSave = 1; //the player starts facing to the right
        deathButton.onClick.AddListener(DeathButtonPressed);
        shieldSprite = shield.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KillPlayerUponEnteringLaserIfNotDashing(collision);
        KillPlayerUponTouchingWater(collision);
    }

    private void KillPlayerUponTouchingWater(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            Die();
        }
    }

    private void KillPlayerUponEnteringLaserIfNotDashing(Collider2D collision)
    {
        if (collision.gameObject.tag == "dashThrough" && !isDashing)
        {
            Die();
        }
    }

    private void Die()
    {
        deathButton.gameObject.SetActive(true);
        Time.timeScale = 0; //pause the game until the respawn button is pressed
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
        Time.timeScale = 1; //unpause game after respawning
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();
        SetHorizontalVelocity();
        Jump();
        Dash();
        ShowShieldSpriteWhileDashing();
        MovementDependingOnIfDashing();
    }

    private void ShowShieldSpriteWhileDashing()
    {
        if (isDashing)
        {
            shieldSprite.enabled = true;
        }
        else
        {
            shieldSprite.enabled = false;
        }
    }

    private void UpdateIsGrounded()
    {
        isGrounded = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;
        if (isGrounded)
        {
            canDash = true; //landing on ground resets dash

            animator.SetBool("isNotOnGround", false);
            speed = speedSave;
        }
        else
        {
            speed = halfSpeedSave;
            animator.SetBool("isNotOnGround", true);
        }
    }

    /// <summary>
    /// If you aren't dashing, you move by setting velocity.
    /// If you are dashing, continue to dash for dashDuration then stop dashing, unfreeze Y position, and reset dashDuration
    /// </summary>
    private void MovementDependingOnIfDashing()
    {
        if (!isDashing)
        {
            rb.velocity = velocity;
            animator.SetFloat("Speed", Mathf.Abs(velocity.x));
            MirrorPlayerSpriteWhenDirectionChanges();
        }
        else
        {
            StopDashing();
        }
    }

    private void StopDashing()
    {
        dashDuration -= Time.deltaTime;
        if (dashDuration < 0)
        {
            isDashing = false;
            dashDuration = dashDurationSave;
            UnfreezeY();
            animator.SetBool("isDashing", false);
        }
    }

    /// <summary>
    /// Faces the player sprite in the direction the player has moved
    /// </summary>
    private void MirrorPlayerSpriteWhenDirectionChanges()
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

    /// <summary>
    /// press shift to dash in the direction you're facing if you haven't dashed before and you are not grounded
    /// </summary>
    private void Dash()
    {
        if (Input.GetButton("Fire3") && canDash && !isGrounded)
        {
            dashVelocity = horizontalRawAxisValueSave * speedSave * dashsSpeedMultiplier;
            FreezeY();
            rb.AddForce(new Vector2(dashVelocity, 0));
            isDashing = true;
            canDash = false;
            animator.SetBool("isDashing", true);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpHeight;
        }
    }

    /// <summary>
    /// xAxis is the velocity on the x axis
    /// This takes the player's velocity in all directions and overrides x velocity
    /// </summary>
    private void SetHorizontalVelocity()
    {
        xAxis = Input.GetAxisRaw("Horizontal") * speed;
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
