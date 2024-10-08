using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Method to be called by the Animation Event
    public GameObject explosionPrefab; // Explosion effect for the player

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("ExplosionPrefab is not assigned.");
            }
            Destroy(gameObject);
        }
    }
}
