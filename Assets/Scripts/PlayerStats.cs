using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerStats
{
    public string userName;
    public int totalTimeSpent;
    public int highscore;
    public int xp;
    public long updatedOn;
    public long createdOn;

    public PlayerStats()
    {

    }

    public PlayerStats(string userName, int highScore, int xp = 0, int totalTimeSpent = 0)
    {
        this.userName = userName;
        this.highscore = highScore;
        this.xp = xp;
        this.totalTimeSpent = totalTimeSpent;

        var timestamp = this.GetTimeUnix();
        this.updatedOn = timestamp;
        this.createdOn = timestamp;
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }


    public string PlayerStatsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
