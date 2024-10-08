using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public float speed = 5f;  // Speed at which the background moves
    public float resetPosition = -20f; // The position at which the background resets (left side)
    public float startPosition = 20f;  // The position where the background will reappear (right side)

    void Update()
    {
        // Move the background to the left continuously
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // If the background goes beyond the reset position (off-screen), reset its position
        if (transform.position.x <= resetPosition)
        {
            Vector3 newPosition = new Vector3(startPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
