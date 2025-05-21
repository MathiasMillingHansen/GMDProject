using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Text healthText;

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

        currentHealth = maxHealth;

        UpdateHealthUI();

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
            controls = new PlayerControls(); 
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
            rb.linearVelocity = new Vector2(speed * moveDirection, rb.linearVelocity.y); // Move the player while maintaining vertical velocity
            transform.localScale = new Vector3(Mathf.Sign(moveDirection) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1); // Flip the player based on movement direction
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
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
            animator.SetBool("isJumping", false); 
            animator.SetBool("isFalling", true);  
            hasFallen = true;  
        }
        else if (isGrounded && hasFallen)
        {
            animator.SetBool("isFalling", false); // Stop the falling animation when grounded
            hasFallen = false;  
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 

            
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
        SceneManager.LoadScene("WinScreen"); 
    }
}


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; 
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        UpdateHealthUI(); 

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
        isInvincible = true; 
        yield return new WaitForSeconds(iframeDuration); 
        isInvincible = false; 
    }

    private void UpdateHealthUI()
{
    if (healthText != null)
    {
        healthText.text = "HP: " + currentHealth;
    }
}

    
    private void Die()
    {
        SceneManager.LoadScene("DiedScreen");
    }
}