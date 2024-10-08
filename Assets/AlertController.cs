using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlertController : MonoBehaviour
{
    public GameObject alertPanel; 
    public Button replayButton; 
    public float delayBeforeContinue = 3f; 
    public Image[] lifeIcons; 
    public ObstacleSpawner obstaclespawner;
    public GameObject successPanel;

    private List<GameObject> disabledObjects = new List<GameObject>(); 

    void Start()
    {
        alertPanel.SetActive(false);
        obstaclespawner = FindObjectOfType<ObstacleSpawner>(); 
    }

    private void Update()
    {

    }

    public void HideAlert() {
        alertPanel.SetActive(false);
    }

    public void ShowAlert()
    {
        if (successPanel != true)
            StartCoroutine(PauseGameAfterDelay(0));    
    }

    public void OnReplayButtonClicked()
    {
        StartCoroutine(RestartGameAfterDelay(0));
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        alertPanel.SetActive(false);
        
        yield return new WaitForSecondsRealtime(delay);
        
        ReactivateObjects();
        
        Time.timeScale = 1;
        obstaclespawner.StartSpawning();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PauseGameAfterDelay(float delay)
    {
        alertPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(delay);
    }

    void PauseGame()
    {
        alertPanel.SetActive(true);
        
        Time.timeScale = 0;
    }

    private void HideObjects()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.activeSelf)
            {
                disabledObjects.Add(obj);
                obj.SetActive(false);
            }
        }
    }

    private void ReactivateObjects()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle != null)
                obstacle.gameObject.SetActive(true);
        }
        
        foreach (Image lifeIcon in lifeIcons)
        {
            if (lifeIcon != null)
                lifeIcon.gameObject.SetActive(true);
        }
        
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        foreach (GameObject explosion in explosions)
        {
            if (explosion != null)
                Destroy(explosion);
        }
    }
}
