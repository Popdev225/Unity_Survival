using UnityEngine;
using UnityEngine.UI;

public class LiveBarController : MonoBehaviour
{
    public Slider slider; 
    public int maxLives = 3; 
    private int currentLives;
    private int crashCount;

    void Start()
    {
        currentLives = maxLives;
        crashCount = 0;
        UpdateLiveBar();
    }

    public void ReduceLives()
    {
        if (currentLives > 0)
        {
            currentLives--;
            crashCount++;

            if (currentLives == 0)
            {
                crashCount = 0;
                currentLives = maxLives;
                
                UpdateLiveBar();
            }
            else if (currentLives > 4)
            {
                UpdateLiveBar();
            }
        }
    }

    private void UpdateLiveBar()
    {
        slider.value = maxLives - crashCount;
    }
}
