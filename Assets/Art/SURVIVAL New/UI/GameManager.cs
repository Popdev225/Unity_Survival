using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button homeButton;
    void Start () {
        DisableButtons();
    }
    // You can assign these in the Inspector if you have UI buttons
    public void StartGame()
    {
        Time.timeScale = 1;
        // Load the main game scene
        // Replace "GameScene" with the name of your actual game scene
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        // If in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If running a built game, quit the application
        Application.Quit();
        #endif
    }

    public void Continue() {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void DisableButtons()
    {
        if (homeButton != null)
        {
            homeButton.interactable = false; // Disables the button
        }
    }

 
}