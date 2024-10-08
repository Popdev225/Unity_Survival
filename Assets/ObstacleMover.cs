using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 2f;
    public float verticalDistance = 1f;
    public float verticalSpeed = 1f;
    public float boundaryX = 10f;
    public float avoidanceRadius = 2f;
    public LayerMask obstacleLayer;

    private float direction = 1f;
    private Vector3 startPosition;
    private float initialVerticalOffset;

    public float explosionForce = 1000f;
    public float explosionRadius = 5f;
    public float explosionDuration = 0.5f;
    public GameObject explosionPrefab;
    public GameObject[] explosions;
    private float elapsedTime;

    void Start()
    {
        startPosition = transform.position;

        initialVerticalOffset = Random.Range(0f, 2 * Mathf.PI);

        direction = Random.value > 0.5f ? 1f : -1f;

        StartCoroutine(InvokeEveryInterval());
    
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (transform.position.x > 10f || transform.position.x < -10f)
        {
            direction *= -1f;
        }

        float newY =
            startPosition.y
            + Mathf.Sin(Time.time * verticalSpeed + initialVerticalOffset) * verticalDistance;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        AvoidOverlap();

        if (Mathf.Abs(transform.position.x) > boundaryX)
        {
            Destroy(gameObject);
        }


    }

    private IEnumerator increaseSpeed()
    {
        yield return new WaitForSecondsRealtime(2f);
    }

    void AvoidOverlap()
    {
        Collider2D[] nearbyObstacles = Physics2D.OverlapCircleAll(
            transform.position,
            avoidanceRadius,
            obstacleLayer
        );

        foreach (Collider2D obstacle in nearbyObstacles)
        {
            if (obstacle.gameObject != gameObject)
            {
                Vector3 directionAway = transform.position - obstacle.transform.position;
                transform.position += directionAway.normalized * speed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship") || collision.gameObject.CompareTag("Bullet"))
        {
            if (explosionPrefab != null)
            {
                GameObject obstacleExplosion = Instantiate(
                    explosionPrefab,
                    gameObject.transform.position,
                    Quaternion.identity
                );
                Destroy(obstacleExplosion, explosionDuration);
                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
    }

    public void resetVerticalSpeed()
    {
        verticalSpeed = 2f;
        speed += 0.5f;
    }

    IEnumerator InvokeEveryInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(0f);
            // Call the function you want to invoke every 30 seconds
            speed = 2f;
        }
    }
}
