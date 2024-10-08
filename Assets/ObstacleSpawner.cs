using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; 
    public float minSpawnInterval = 5f; 
    public float maxSpawnInterval = 5f; 
    public float spawnRangeY = 4f; 
    public float spawnRangeX = 4f; 
    public int minObstacles = 20; 
    public int maxObstacles = 300; 
    public float minDistanceBetweenObstacles = 1.5f; 
    public int maxActiveObstacles = 20; 
    public bool debugMode = false; 

    private float screenRightX; 
    private bool isSpawning = true; 
    private bool increasespeed = false;

    void Start()
    {
        screenRightX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        StartCoroutine(SpawnObstaclesCoroutine());
    }

    public IEnumerator SpawnObstaclesCoroutine()
    {
        while (isSpawning)
        {
            if (GameObject.FindGameObjectsWithTag("Obstacle").Length < maxActiveObstacles)
            {
                int numberOfObstacles = Random.Range(minObstacles, maxObstacles + 1);
                
                List<Vector2> spawnPositions = new List<Vector2>();

                for (int i = 0; i < numberOfObstacles; i++)
                {
                    Vector2 spawnPosition;
                    bool positionIsValid;

                    int attempts = 0;
                    const int maxAttempts = 20; 
                    do
                    {
                        float yPosition = Random.Range(-spawnRangeY, spawnRangeY);
                        float xPosition = screenRightX + Random.Range(0, spawnRangeX);
                        spawnPosition = new Vector2(xPosition, yPosition);
                        positionIsValid = true;
                        
                        foreach (Vector2 existingPosition in spawnPositions)
                        {
                            if (Vector2.Distance(existingPosition, spawnPosition) < minDistanceBetweenObstacles)
                            {
                                positionIsValid = false;
                                break;
                            }
                        }

                        attempts++;
                        if (attempts >= maxAttempts && debugMode)
                        {
                            Debug.LogWarning("Couldn't find a valid spawn position.");
                        }
                    } while (!positionIsValid && attempts < maxAttempts);

                    if (positionIsValid)
                    {
                        spawnPositions.Add(spawnPosition);

                        if (obstaclePrefabs.Length > 0)
                        {
                            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
                            
                            GameObject obstacle = Instantiate(obstaclePrefabs[randomIndex], spawnPosition, Quaternion.identity);

                             if (increasespeed)
                             {
                                ObstacleMover obstacleScript = obstacle.GetComponent<ObstacleMover>();
                                obstacleScript.resetVerticalSpeed();
                             }

                            obstacle.tag = "Obstacle"; 
                        }
                    }
                }
            }
            
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnObstaclesCoroutine()); 
    }

    public void reset() {
        maxActiveObstacles = 20;
    }
    public void increaseObstacles() {
        maxActiveObstacles += 40;
         increasespeed = true;
    }
}
