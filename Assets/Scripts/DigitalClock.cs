using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    public const int hoursInDay = 24, minutesInHour = 60;

    public float dayDuration = 30f;

    float totalTime = 0;
    float currentTime = 0;

    public TextMeshProUGUI clock;

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        currentTime = totalTime % dayDuration;

        clock.text = Clock24hour();
    }

    public float GetHour()
    {
        return currentTime * hoursInDay / dayDuration;
    }

    public float GetMinutes()
    {
        return (currentTime * hoursInDay * minutesInHour / dayDuration) % minutesInHour;
    }

    public string Clock24hour()
    {
        return Mathf.FloorToInt(GetHour()).ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00");
    }
}
