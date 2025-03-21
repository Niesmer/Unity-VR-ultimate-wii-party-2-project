using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10f;
    public bool timerIsRunning = false;
    public bool isCountDown = true;
    public List<TMP_Text> timeTexts;

    void Start()
    {
        DisplayTime(timeRemaining);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (isCountDown)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    DisplayTime(timeRemaining);
                }
            }
            else
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (isCountDown)
        {
            timeToDisplay = Mathf.Max(0, timeToDisplay);
        }

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000);

        string timeString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        foreach (TMP_Text timeText in timeTexts)
        {
            timeText.text = timeString;
        }
    }
}