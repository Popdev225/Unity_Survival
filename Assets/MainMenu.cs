using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject exitButton;
    public GameObject scorePanel;

    void Start()
    {
#if UNITY_WEBGL
        exitButton.SetActive(false);
#endif
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
        
        SceneManager.LoadScene("MainScene");
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        
        #else
        
        Application.Quit();
        #endif
    }

  
}
