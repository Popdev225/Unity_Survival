using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    public Slider speedBar;
    public Speed playerSpeed;
    public Image fill; // Reference to the Image component of the fill area

    private void Start()
    {
        playerSpeed = GameObject.FindGameObjectWithTag("Ship").GetComponent<Speed>();
        speedBar = GetComponent<Slider>();

        // Automatically assign the fill image if it's not assigned manually
        if (fill == null)
        {
            fill = speedBar.fillRect.GetComponent<Image>();
        }

        speedBar.maxValue = playerSpeed.maxSpeed;
        speedBar.value = 1;
    }

    private void UpdateSpeedBarColor()
    {
        if (speedBar.value <= 2)
        {
            fill.color = Color.yellow;
        }
        else
        {
            fill.color = Color.blue; // Default color, change as needed
        }
    }

    public void SetSpeed(int hp)
    {
        speedBar.value = hp;
        UpdateSpeedBarColor();
    }
}
