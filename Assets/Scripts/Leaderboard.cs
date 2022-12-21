using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Leaderboard
{
    public string userName;
    public int highScore;
    public long updatedOn;

    public Leaderboard()
    {

    }

    public Leaderboard(string userName, int highScore)
    {
        this.userName = userName;
        this.highScore = highScore;
        this.updatedOn = GetTimeUnix();
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    public string LeaderboardToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
