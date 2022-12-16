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

        clock.text = Clock12Hour();
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
        return Mathf.FloorToInt(GetHour()).ToString("12") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00");
    }

    public string Clock12Hour()
    {
        int hour = Mathf.FloorToInt(GetHour());
        string abbreviation = "PM";

        if (hour >= 12)
        {
            abbreviation = "AM";
            hour -= 12;
        }

        if (hour == 0) hour = 12;
        return hour.ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00") + " " + abbreviation;
    }
}
