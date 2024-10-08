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

public class GetScores : MonoBehaviour
{

    private const string leaderboardId = "survival_leaderboard";  // Replace with your leaderboard ID
    public Transform tableContent;      // UI Panel or parent for holding leaderboard rows
    public GameObject tableRowPrefab;   // Prefab with Text fields for No, Name, Score
    public Color oddRowColor;             // Color for odd rows
    public Color evenRowColor;            // Color for even rows

    // Start is called before the first frame update
    async void Start()
    {
        await Initialize();
        await DisplayTopScores(5);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public async Task Initialize()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Failed to authenticate: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

 // Fetch and display the top scores from the leaderboard
 public async Task DisplayTopScores(int limit = 5)
    {
        try
        {
            var response = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = limit });

            Debug.Log("Top Players:");
            foreach (var entry in response.Results)
            {
                string playerName = await LoadPublicDataByPlayerId(entry.PlayerId);
                Debug.Log(playerName);
            }
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Error fetching top scores: {ex.Message}, Error Code: {ex.ErrorCode}");
        }
    }

 public async Task<string> LoadPublicDataByPlayerId(string playerId)
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{"DisplayName"}, new LoadOptions(new PublicReadAccessClassOptions(playerId)));


    // Check if "DisplayName" exists in the playerData and return it
    if (playerData.TryGetValue("DisplayName", out var DisplayName))
    {
        Debug.Log($"DisplayName: {DisplayName.Value.GetAs<string>()}");
        return DisplayName?.Value?.GetAs<string>() ?? "default_value";
    }
    else
    {
        Debug.LogWarning("DisplayName not found");
        return null; // or return an empty string if preferred
    }
}
    
}
