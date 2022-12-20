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

    public bool timeMoving = true;

    // Update is called once per frame
    void Update()
    {
        if (timeMoving)
        {
            totalTime += Time.deltaTime;
            currentTime = totalTime % dayDuration;

            clock.text = Clock12Hour();
        }
    }

    public float GetHour()
    {
        return currentTime * hoursInDay / dayDuration;
    }

    public float GetMinutes()
    {
        return (currentTime * hoursInDay * minutesInHour / dayDuration) % minutesInHour;
    }

    public string Clock12Hour()
    {
        int hour = Mathf.FloorToInt(GetHour());
        string abbreviation = "PM";

        if (hour == 6)
        {
            timeMoving = false;
            GameManager.instance.DayEnd();
            return "06:00 " + abbreviation;
        }

        if (hour == 0) hour = 12;
        return hour.ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00") + " " + abbreviation;
    }
}
