using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the player moves
    public float accelerationTime = 0.01f; // Time to reach max speed
    public float decelerationTime = 0.01f; // Time to stop completely
    private Rigidbody2D rb; // Reference to Rigidbody2D
    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public Transform shootPoint; // The point from which the bullet is shot (e.g., in front of the player)
    private Vector2 targetVelocity; // The target velocity the player is moving towards

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get raw input (instant input without smoothing)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow keys
        float vertical = Input.GetAxisRaw("Vertical"); // W/S or Up/Down Arrow keys

        // Create a target velocity based on input (normalized to avoid faster diagonal movement)
        targetVelocity = new Vector2(horizontal, vertical).normalized * moveSpeed;

        // Handle acceleration and deceleration with lerp for instant but smooth changes
        if (horizontal != 0 || vertical != 0)
        {
            // Accelerate quickly (instant reaction)
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime / accelerationTime);
        }
        else
        {
            // Decelerate quickly to stop
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime / decelerationTime);
        }
        
        if (Input.GetButtonDown("Fire1")) 
            Shoot();
    }
    void Shoot()
    {
        // Instantiate the bullet at the shoot point
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            
        // Determine direction: shoot based on player's movement direction
        Vector2 shootDirection = targetVelocity.normalized;

        // If no movement input, shoot straight (to avoid zero velocity errors)
        if (shootDirection == Vector2.zero)
        {
            shootDirection = Vector2.up; // Default to upwards if the player isn't moving
        }

        // Call the Fire method on the bullet to move it in the correct direction
        bullet.GetComponent<Bullet>().Fire(shootDirection);
    }
}