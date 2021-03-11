using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManipulator : MonoBehaviour
{
    [Range(0.5f, 20f)] public float timeScale = 1;

    private float baseTimeScale;
    private float baseFixedUpdateRate;

    public float timerValue;
    public Text timerText;
    void Start()
    {
        baseTimeScale = Time.timeScale;
        baseFixedUpdateRate = Time.fixedDeltaTime * 1;
    }

    private void Update()
    {
        Time.timeScale = timeScale * baseTimeScale;
        Time.fixedDeltaTime = baseFixedUpdateRate * (1 / timeScale);

        if (timerValue >= 0)
        {
            timerValue -= Time.deltaTime;
            timerText.text = Math.Round(timerValue, 1, MidpointRounding.AwayFromZero).ToString();
        }
            
    }
}
