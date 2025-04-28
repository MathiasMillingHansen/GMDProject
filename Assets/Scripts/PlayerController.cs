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

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool hasFallen; // Track if the falling animation has already been triggered

    private void Awake()
    {
        controls = new PlayerControls();
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

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Move();
        HandleFalling();
    }

    private void Move()
    {
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
        Debug.Log("Player fell off the map!");
        Die();
    }

    if (collision.CompareTag("Winbox"))
    {
        Debug.Log("Player reached the win box!");
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

    // Function to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to handle player death
    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene("DiedScreen"); // Load the DiedScreen scene
    }
}