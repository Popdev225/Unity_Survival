using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.Services.Leaderboards;
using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.CloudSave;  // Fixed this for cloud saving
using Unity.Services.Authentication;
using System.Collections.Generic;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.CloudSave.Models;

public class GameTimer : MonoBehaviour
{
    public GameObject successPanel;
    public PlayerController playergame;
    public AdvancedShipController updatedplayergame;
    public GameObject replayPanel;
    public GameObject dommagePanel;
    public GameObject player;
    public ObstacleMover obstacleMover;
    public Text timeText;
    public int currentLives = 3;
    public float elapsedTime;
    private bool isGameRunning = true;
    private float gameDuration = 90f;
    public float escapeSpeed = 20f;
    public bool gameFinal = false;
    public int timesupdate = 0;

    public GameObject SendPanel;

    public long playerScore;  // Score is a float here
    public ObstacleSpawner obstaclespawner;
    private const string leaderboardId = "survival_leaderboard";  // Replace with your leaderboard ID

    public InputField nameInputField;  // Attach your NameInputField here
    public InputField emailInputField; // Attach your EmailInputField here
    public Button submitButton;        // Attach the SubmitButton here
    public Text displayText;           // Optional: Text to display results
    
    // Initialization and Start Method
    async void Start()
    {
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitButtonClick);

        playergame = FindObjectOfType<PlayerController>();
        obstaclespawner = FindObjectOfType<ObstacleSpawner>();
        obstacleMover = FindObjectOfType<ObstacleMover>();

           // Authenticate the player
        await Initialize();

        // Save player profile (name, email, score)
        
        // Load and display player profile
        //await LoadPlayerProfile();

    }

     public async Task Initialize()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Player authenticated. Player ID: " + AuthenticationService.Instance.PlayerId);
            }
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Failed to authenticate: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

    // Method to save the player's profile (name, email, score)
    public async Task SavePlayerProfile(string displayName, string email, long score)
    {
        try
        {
            // Creating a dictionary to store player data
            var playerData = new Dictionary<string, object>
            {
                { "DisplayName", displayName },
                { "Email", email },
                { "Score", score }
            };

            // Saving player data using Cloud Save
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData, new SaveOptions(new PublicWriteAccessClassOptions()));
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Failed to save player profile: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

        // This function is called when the Submit button is clicked
    private async void OnSubmitButtonClick()
    {
        // Get the input values from the input fields
        string playerName = nameInputField.text;
        string playerEmail = emailInputField.text;

        // Submit the player's score to the leaderboard
        await SubmitScore((int)elapsedTime);

        await SavePlayerProfile(playerEmail, playerName, (int)elapsedTime);

      // FetchMultiplePlayersData();

        SceneManager.LoadScene("MainMenu");
    }

    // Submit the player's score to the leaderboard
    public async Task SubmitScore(long score)
    {
        try
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
            Debug.Log("Score submitted successfully to the leaderboard!");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Error submitting score: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

    private void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        // Check if updatedplayergame has become active and gameOver is true
        if (updatedplayergame != null && updatedplayergame.gameOver && timesupdate == 0)
        {
            timesupdate += 1;
            EndGame();  // Call any end game logic
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Samsung"))
        {
            HandleCollisionWithSamsung();
        }
    }

    private void HandleCollisionWithSamsung()
    {
        currentLives--;

        if (currentLives <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        isGameRunning = false;

        float finalTime = elapsedTime;
        string formattedTime = string.Format("{0:F2} SECONDS", finalTime);
        float gb = finalTime * 7.4f;
        string formatGB = string.Format("{0}", gb);

        if (successPanel != null && timeText != null)
        {
            obstaclespawner.StopSpawning();
            timeText.text =
                $"Vous avez survécu {formattedTime}. Sachez que pendant ce temps, le SSD 990 PRO avec dissipateur Samsung a pu lire {formatGB} ";

            // Convert to seconds and milliseconds
            int seconds = (int)elapsedTime;  // Get the whole seconds part
            int milliseconds = (int)((elapsedTime - seconds) * 1000);  // Get the milliseconds part

            displayText.text = 
                            $"{seconds} : {milliseconds}ms";

             //yield return new WaitForSecondsRealtime(2f);
             StartCoroutine(SuccessAlert(2f));
             
        }
    }

    private IEnumerator SuccessAlert(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        
        successPanel.SetActive(true);

    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
