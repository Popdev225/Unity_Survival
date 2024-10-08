using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class AdvancedShipController : MonoBehaviour
{
    public float explosionDuration = 0.4f; 
    public GameObject explosionPrefab; 
    public GameObject[] explosions;
    public int crashCount = 0;
    private int maxCrashes = 27; 
    public Speed speed;
    public SpeedBar speedbar;
    public AlertController alertController;
    public ObstacleSpawner obstaclespawner;
    public Image[] lifeIcons; 
    public GameObject[] obstacles;
    public Sprite fullLifeSprite; 
    public Sprite lostLifeSprite; 
    public GameObject replayPanel;
    public Button replayButton; 
    public Button restartButton; 
    public Button finishButton; 
    public GameObject finish; 
    public GameObject samsungPrefab; 
    public GameObject floatingTextPrefab; 
    private GameObject currentSamsung; 
    private bool isSamsungActive = false; 
    public int stateReset = 0;
    public GameObject[] liveIcons; 
    public int currentLives;
    public int currentSpeed;
    private Camera mainCamera;
    private float screenWidth;
    private float screenHeight;
    public Button exitButton; 
    public Button continueButton; 
    public Button homeButton; 
    public GameObject home; 
    public GameObject exit; 
    public GameObject gameExitPanel; 
    private Animator animator;
    public GameObject[] speedClipIcons; 
    public GameObject speedBar;
    public int currentSpeedClip = 0;
    public Color fullHealthColor = Color.green;
    public Color halfHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    public GameObject[] healthClipIcons; 
    public GameObject healthBar;
    public int currentHealthClip = 0;
    public GameObject livePanel;
    public GameObject Thruster1;
    public GameObject Thruster2;
    public GameObject dommagePanel; 
    public Button restartButton1; 
    public Button exitButton1; 
    public Button goHomeButton; 
    public GameObject goHome; 
    public int xCount = 0;
    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint; 
    public float bulletSpeed = 5f; 
    private float nextFireTime = 0f; 
    public float escapeSpeed = 20f;
    public GameObject FlyBack;
    public Sprite newSprite; 
    public bool isInvincible = false; 
    public float invincibilityDuration = 1f; 
    public float flashDuration = 0.1f; 
    private SpriteRenderer spriteRenderer;
    private bool collisionBool = true;
    public int round = 2;
    public GameObject oldShip;
    public GameObject successPanel;
    public bool gameOver = false;
    public int obSpeed = 0;
    public ObstacleMover[] movingObjects;
    public int level = 0;

    void Start()
    {
        mainCamera = Camera.main;
        currentHealthClip = 9;
        currentSpeedClip = 3;
        currentLives = 3; 
        maxCrashes = 3;

        oldShip.SetActive(false);
        obstaclespawner.reset();
        StartCoroutine(InvokeEveryInterval());
        obstaclespawner.StartSpawning();
        StartCoroutine(obstaclespawner.SpawnObstaclesCoroutine());

        UpdateHealthAndLivesUI();
        
        animator = GetComponent<Animator>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        screenHeight = mainCamera.orthographicSize;
        screenWidth = screenHeight * screenRatio;

        alertController = FindObjectOfType<AlertController>(); 
        obstaclespawner = FindObjectOfType<ObstacleSpawner>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (replayButton != null)
            replayButton.onClick.AddListener(OnReplayButtonClicked);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGameFromFirst);

        if (exitButton != null)
            exitButton.onClick.AddListener(QuitGame);

        if (homeButton != null)
            homeButton.onClick.AddListener(restartHome);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueGame);

        if (finishButton != null)
            finishButton.onClick.AddListener(QuitGame);

        if (restartButton1 != null)
            restartButton1.onClick.AddListener(ReturnHome);
        
        if (exitButton1 != null)
            exitButton1.onClick.AddListener(QuitGame);

        if (goHomeButton != null)
            goHomeButton.onClick.AddListener(ReturnHome);

            // Find all GameObjects with the MoveObject script
        movingObjects = GameObject.FindObjectsOfType<ObstacleMover>();

        
    }

    public void RestartNewGameFromFirst()
    {
        Time.timeScale = 1;
        dommagePanel.SetActive(false);

        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(currentSceneName);
    }

    public void ChangeSprite()
    {
        
        if (newSprite != null)
            spriteRenderer.sprite = newSprite;
        else
            Debug.LogError("New sprite not assigned in the Inspector!");
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;

        Color targetColor;
        if (healthPercentage > 0.5f)
        {
            targetColor = Color.Lerp(
                halfHealthColor,
                fullHealthColor,
                (healthPercentage - 0.5f) * 2
            );
        }
        else
        {
            targetColor = Color.Lerp(lowHealthColor, halfHealthColor, healthPercentage * 2);
        }

        foreach (GameObject go in healthClipIcons)
        {
            Image image = go.GetComponent<Image>();
            if (image != null)
            {
                image.color = targetColor;
            }
            else
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = targetColor;
                }
            }
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
#else
        
        Application.Quit();
#endif
    }

    void ContinueGame()
    {
        EnableButtons();
        Time.timeScale = 1;
        gameExitPanel.SetActive(false);
    }

    void ReturnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void restartHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    void UpdateLiveIcons()
    {
        for (int i = 0; i < liveIcons.Length; i++)
        {
            if (i < currentLives)
            {
                liveIcons[i].SetActive(true);
            }
            else
            {
                liveIcons[i].SetActive(false);
            }
        }
    }

    void UpdateHealthClipIcons()
    {
        for (int i = 0; i < healthClipIcons.Length; i++)
        {
            if (i < currentHealthClip)
            {
                healthClipIcons[i].SetActive(true);
            }
            else
            {
                healthClipIcons[i].SetActive(false);
            }
        }
    }

    void UpdateSpeedClipIcons()
    {
        for (int i = 0; i < speedClipIcons.Length; i++)
        {
            if (i < currentSpeedClip)
            {
                speedClipIcons[i].SetActive(true);
            }
            else
            {
                speedClipIcons[i].SetActive(false);
            }
        }
    }



    void ResetHealthAndLives()
    {
        currentLives = 3;
        maxCrashes = 3;
        currentHealthClip = 9;
        currentSpeedClip = 2;
        stateReset = 0;
    }

    public void RestartGameFromFirst()
    {
        Time.timeScale = 1;
     //   SetVisibility(true);
        successPanel.SetActive(false);
        //SceneManager.LoadScene("MainMenu");
    }

    public void OnReplayButtonClicked()
    {
        StartCoroutine(RestartGameAfterDelay(2));
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        Time.timeScale = 1; 
        alertController.HideAlert();
        SetVisibility(true);

        yield return new WaitForSeconds(2f);
        
        crashCount = 0;
        stateReset += 1;
        
        currentLives = 3; 
        currentHealthClip = 1; 
        currentSpeedClip = 1;

        Thruster1.SetActive(true);
        Thruster2.SetActive(false);
        
        ActivateAllObstacles();
        UpdateLivesUI(); 
        UpdateHealthClipIcons(); 
        UpdateSpeedClipIcons();
        UpdateHealthBar(currentHealthClip, 9);

        yield return new WaitForSeconds(4f);

        if (obstaclespawner != null)
        {
            obstaclespawner.reset();
            obstaclespawner.StartSpawning();
            StartCoroutine(obstaclespawner.SpawnObstaclesCoroutine());
        }
    }

    void UpdateHealthAndLivesUI()
    {
        UpdateHealthClipIcons();
        UpdateLivesUI();
        UpdateSpeedClipIcons();
        UpdateHealthBar(currentHealthClip, 9);
    }

    public void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < currentLives)
            {
                lifeIcons[i].sprite = fullLifeSprite; 
            }
            else
            {
                lifeIcons[i].sprite = lostLifeSprite; 
            }
        }
    }

    private void ActivateAllObstacles()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }
        
        if (speedBar != null)
        {
            speedBar.gameObject.SetActive(true);
        }

        foreach (Image lifeIcon in lifeIcons)
        {
            if (lifeIcon != null)
            {
                lifeIcon.gameObject.SetActive(true);
                lifeIcon.sprite = fullLifeSprite; 
            }
        }

        if (livePanel != null)
        {
            livePanel.gameObject.SetActive(true);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            bulletSpawnPoint.position,
            Quaternion.identity
        );
        
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(bulletSpeed, 0); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime && collisionBool) 
        {
            Shoot();
            nextFireTime = Time.time; 
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!replayPanel.activeInHierarchy && !dommagePanel.activeInHierarchy)
            {
#if UNITY_WEBGL
                
                home.SetActive(true);
                exit.SetActive(false);
#endif
                EnableButtons();
                gameExitPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (gameOver)
        {
            Escape();
        }


        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetTrigger("MoveDownTrigger");
            animator.SetBool("IsMovingDown", true);
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetTrigger("MoveUpTrigger");
            animator.SetBool("IsMovingUp", true); 
        }
        
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetBool("IsMovingDown", false);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            animator.SetBool("IsMovingUp", false);
        }

        Vector3 position = transform.position;
        
        position.x = Mathf.Clamp(position.x, -screenWidth, screenWidth + 2);
        position.y = Mathf.Clamp(position.y, -screenHeight, screenHeight);

        transform.position = position;
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        transform.Translate(movement * currentSpeedClip * Time.deltaTime);


    }

    IEnumerator InvokeEveryInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            // Call the function you want to invoke every 30 seconds
            obstaclespawner.increaseObstacles();
          //  Time.timeScale += 0.5f;

            level = 1;

            //IncreaseObstacleSpeed();
            // Wait for the interval (30 seconds)
            obSpeed += 1;
        }
    }

    public void IncreaseObstacleSpeed()
    {
        // Find all GameObjects that have the ObstacleMovement script
        ObstacleMover[] obstacles = FindObjectsOfType<ObstacleMover>();

        // Loop through each one and increase its speed
        foreach (ObstacleMover obstacle in obstacles)
        {
            obstacle.resetVerticalSpeed();
        }
    }

    private IEnumerator FlashAndInvincible()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += flashDuration * 2)
        {
            spriteRenderer.enabled = false;
            FlyBack.GetComponent<Renderer>().enabled = false;
            Thruster2.GetComponent<Renderer>().enabled = false;
            Thruster1.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.enabled = true;
            FlyBack.GetComponent<Renderer>().enabled = true;
            Thruster2.GetComponent<Renderer>().enabled = true;
            Thruster1.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(flashDuration);
        }

        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            crashCount++;
            HandleCrash(collision.gameObject);
        }

    }

    private IEnumerator ShipUpdateAnimation()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        Thruster1.SetActive(false);
        Thruster2.SetActive(true);

        //FlyBack.SetActive(true);
    }
public void Escape()
{
    animator.SetTrigger("ShipGo");

    // Test with a higher escapeSpeed if necessary
    transform.Translate(Vector2.right * escapeSpeed * Time.deltaTime);
    
    // Check if the object has moved past the camera's right boundary
    if (transform.position.x > Camera.main.orthographicSize * Camera.main.aspect + 1f)
    {
        SetVisibility(false);
    }

    // Disable Thruster2 after the escape
    Thruster2.SetActive(false);
}

    void SetVisibility(bool visible)
    {
        Renderer shipRenderer = GetComponent<Renderer>();
        if (shipRenderer != null)
            shipRenderer.enabled = visible;

        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
                childRenderer.enabled = visible;
        }
    }

    public void HandleCrash(GameObject obstacle)
    {
        LoseHealthClip();

        UpdateHealthBar(currentHealthClip, 9);
        
        if (crashCount >= maxCrashes)
        {
            if (explosionPrefab != null) 
            {
                //SetVisibility(false);
                // GameObject obstacleExplosion = Instantiate(
                //     explosionPrefab,
                //     gameObject.transform.position,
                //     Quaternion.identity
                // );
                // Destroy(obstacleExplosion, explosionDuration);
            }
            round = 2;
            collisionBool = false;
            ShowGameOverAlert();

            LoseLife();
        }
        else if (
            crashCount != 0 /* && crashCount % 9 == 0*/
        )
        {
            LoseLife();

            currentHealthClip = 9;
            UpdateHealthClipIcons();
            UpdateHealthBar(currentHealthClip, 9);
            StartCoroutine(FlashAndInvincible());
        }
        else if (stateReset == 1)
        {
            collisionBool = false;
            //SetVisibility(false);

            if (explosionPrefab != null) // You might want to use a different explosion prefab for the obstacle
            {
                // GameObject obstacleExplosion = Instantiate(
                //     explosionPrefab,
                //     gameObject.transform.position,
                //     Quaternion.identity
                // );
                // Destroy(obstacleExplosion, explosionDuration);
            }

            stateReset = 0;
            
            ShowGameOverAlert();
        }

        Destroy(obstacle); 
    }

    private void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateLivesUI();
        }
    }

    private void LoseHealthClip()
    {
        if (currentHealthClip > 0)
        {
            currentHealthClip -= 1;
            UpdateHealthClipIcons();
        }
    }

    private void Explode(GameObject obstacle)
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, obstacle.transform.position, Quaternion.identity);
        }
        
        Destroy(obstacle, 0.1f); 
    }

    private void ShowGameOverAlert()
    {
        StartCoroutine(HandleGameOver());
    }

    private void deleteCloneKindof(string name) {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(name);
        foreach (GameObject obj in objs)
        {
            if (obj != null && obj.name.Contains("(Clone)"))
                obj.SetActive(false);
        }
    }

    public void HideAndShowAllObjectsExceptUIAndBackground(bool ctr)
    {
        deleteCloneKindof("Obstacle");

        deleteCloneKindof("Bullet");
    
        if (healthBar != null)
            healthBar.gameObject.SetActive(ctr);
    
        if (speedBar != null)
            speedBar.gameObject.SetActive(ctr);

        foreach (Image lifeIcon in lifeIcons)
        {
            if (lifeIcon != null)
                lifeIcon.gameObject.SetActive(ctr);
        }

        if (livePanel != null)
            livePanel.gameObject.SetActive(ctr);

        if (!ctr)
        {
            deleteCloneKindof("Explosion");
        }
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        //SetVisibility(false);

#if UNITY_WEBGL
        goHome.SetActive(true);
        finish.SetActive(false);
#endif

        if (obstaclespawner != null)
            obstaclespawner.StopSpawning();

        if (currentSamsung != null)
            Destroy(currentSamsung);

        HideAndShowAllObjectsExceptUIAndBackground(false);

        yield return new WaitForSecondsRealtime(1f);
    
        if (xCount == 0)
        {
            gameOver = true;
        }
        else
        {
            dommagePanel.SetActive(true);
            HideAndShowAllObjectsExceptUIAndBackground(true);
        }
    }

    public void OnFinishButtonClicked()
    {
        Application.Quit();
    
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnFinishButton1Clicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void EnableButtons()
    {
        if (continueButton != null)
        {
            continueButton.interactable = true;
        }
    }
}
