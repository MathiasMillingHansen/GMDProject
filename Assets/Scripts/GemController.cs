using UnityEngine;

public class GemController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Gem collided with: " + other.name); // Log the name of the object that collided with the gem

        // Check if the object colliding with the gem is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected the gem!"); // Log that the player collected the gem

            // Access the ScoreManager singleton to update the score
            ScoreManager.Instance.AddScore(500);

            // Destroy the gem
            Destroy(gameObject);
        }
    }
}