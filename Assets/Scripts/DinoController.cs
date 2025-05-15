using UnityEngine;

public class DinoController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; // Movement speed
    private bool movingRight = true; // Direction of movement

    [Header("Damage Settings")]
    public int damageToPlayer = 1; // Damage dealt to the player

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Move the dino in the current direction
        rb.linearVelocity = new Vector2(speed * (movingRight ? -1 : 1), rb.linearVelocity.y);
    }

    private void Flip()
    {
        // Flip the direction
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Flip the sprite
        transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBoundaryBox"))
        {
            // Switch direction when colliding with a boundary box
            Flip();
        }
    }

}