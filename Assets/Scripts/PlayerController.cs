using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerControls controls; // Input System
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 5f;
    private Vector2 moveInput;
    private bool isGrounded;

    [Header("Crouch Settings")]
    public Transform standingCollider;
    public Transform crouchingCollider;

    [Header("Health Settings")]
    public int maxHealth = 3; // Maximum health
    private int currentHealth; // Current health

    [Header("Damage Settings")]
    public float iframeDuration = 0.5f; // Duration of invincibility frames
    private bool isInvincible = false; // Tracks if the player is currently invincible

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool hasFallen; // Track if the falling animation has already been triggered

    private void Awake()
    {
        if (controls == null)
        {
            controls = new PlayerControls(); // Ensure controls are initialized
        }

        rb = GetComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        animator = GetComponent<Animator>();

        // Initialize health
        currentHealth = maxHealth;

        // Check if the OnLandEvent is null
        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        // Input Actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += ctx => { Jump(); };
        controls.Player.Crouch.performed += ctx => Crouch(true);
        controls.Player.Crouch.canceled += ctx => Crouch(false);
    }

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new PlayerControls(); // Reinitialize controls if null
        }
        controls.Enable(); // Enable the input system
    }

    private void OnDisable()
    {
        controls.Disable(); // Disable the input system
    }

    private void Update()
    {
        if (GameController.isPaused) return;
        Move();
        HandleFalling();
    }

    private void Move()
    {

        if (GameController.isPaused) return;


        float moveDirection = moveInput.x;

        if (moveDirection != 0)
        {
            rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y); // Use velocity instead of linearVelocity
            transform.localScale = new Vector3(Mathf.Sign(moveDirection) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Keep the vertical velocity
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
    }

    private void Jump()
    {

        if (GameController.isPaused) return;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
            hasFallen = false; // Reset falling state when jumping
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);  // Stop the falling animation when landed
        hasFallen = false; // Reset falling state on landing
    }

    private void Crouch(bool isCrouching)
    {
        standingCollider.gameObject.SetActive(!isCrouching);
        crouchingCollider.gameObject.SetActive(isCrouching);
        animator.SetBool("Crouch", isCrouching);
    }

    private void HandleFalling()
    {
        // Check if the player is not grounded and falling
        if (!isGrounded && rb.linearVelocity.y < 0 && !hasFallen)
        {
            animator.SetBool("isJumping", false); // Stop the jumping animation when falling
            animator.SetBool("isFalling", true);  // Start the falling animation
            hasFallen = true;  // Mark that falling animation is triggered
        }
        else if (isGrounded && hasFallen)
        {
            animator.SetBool("isFalling", false); // Stop the falling animation when grounded
            hasFallen = false;  // Reset falling state
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set the grounded state when colliding with ground

            // Immediately invoke OnLandEvent
            OnLandEvent.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Killbox"))
    {
        Die();
    }

    if (collision.CompareTag("Winbox"))
    {
        SceneManager.LoadScene("WinScreen"); // Load the WinScreen scene
    }
}

    // Reset isGrounded when leaving the ground
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Set the grounded state to false when leaving the ground
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignore damage if invincible

        currentHealth -= damage;

        // Trigger the hurt animation
        animator.SetBool("BeenHurt", true);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ResetHurtAnimation());
            StartCoroutine(ActivateIframes());
        }
    }

    private IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Short delay to show the hurt animation
        animator.SetBool("BeenHurt", false);
    }

    private IEnumerator ActivateIframes()
    {
        isInvincible = true; // Enable invincibility
        yield return new WaitForSeconds(iframeDuration); // Wait for iframe duration
        isInvincible = false; // Disable invincibility
    }

    // Function to handle player death
    private void Die()
    {
        SceneManager.LoadScene("DiedScreen"); // Load the DiedScreen scene
    }
}