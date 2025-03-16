using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed

    // Called when the bullet is created
    public void Fire(Vector2 direction)
    {
        // Set the bullet's velocity in the direction it's fired
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
    }

    // Called when the bullet collides with something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the bullet when it hits anything
        Destroy(gameObject);
    }

    // Called if the bullet goes off screen (optional)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}