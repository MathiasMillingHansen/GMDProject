using UnityEngine;

public class GemController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        // Check if the object colliding with the gem is the player
        if (other.CompareTag("Player"))
        {

            // Access the ScoreManager singleton to update the score
            ScoreManager.Instance.AddScore(500);

            // Destroy the gem
            Destroy(gameObject);
        }
    }
}