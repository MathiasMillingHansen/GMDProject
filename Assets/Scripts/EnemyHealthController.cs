using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 2; // Maximum health of the enemy
    private int currentHealth;

    [Header("Damage Settings")]
    public float playerBounceForce = 5f; // Force applied to the player when they jump on the enemy
    public int damageToPlayer = 1; // Damage dealt to the player if not jumping on the enemy

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject); 

        ScoreManager.Instance.AddScore(250); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is above the enemy
            if (collision.contacts[0].normal.y < -0.5f)
            {
                // Player jumps on the enemy, no damage to the player
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, playerBounceForce); // Bounce the player up
                }

                TakeDamage(1);
            }
            else
            {
                // Player takes damage if not jumping on the enemy
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damageToPlayer);
                }
            }
        }
    }
}