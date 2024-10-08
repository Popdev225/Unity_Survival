using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public int curSpeed = 0;
    public int maxSpeed = 5;
    public int initialSpeed = 1;
    public SpeedBar speedBar;

    void Start()
    {
        curSpeed = maxSpeed;
        speedBar = FindObjectOfType<SpeedBar>(); // Find the LiveBarController in the scene
    }

    public void ReduceSpeedPlayer(int damage)
    {
        curSpeed = curSpeed - damage;
        Debug.Log("curSpeed:" + curSpeed);
        speedBar.SetSpeed(curSpeed);
        if (curSpeed == 0)
            curSpeed = maxSpeed;
    }

    public void ResetPlayer()
    {
        curSpeed = maxSpeed;
        speedBar.SetSpeed(maxSpeed);
    }
}
