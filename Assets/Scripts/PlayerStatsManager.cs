using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using JetBrains.Annotations;

public class PlayerStatsManager : MonoBehaviour
{
    public TextMeshProUGUI playerXP;
    public TextMeshProUGUI playerTimeSpent;
    public TextMeshProUGUI playerHighScore;
    public TextMeshProUGUI playerLastPlayed;
    public TextMeshProUGUI playerName;

    public MyDatabase fbMgr;
    public AuthManager auth;

    // Start is called before the first frame update
    void Start()
    {

        ResetStatsUI();
        //retrieve current logged in user's uuid
        //update UI
        UpdatePlayerStats(auth.GetCurrentUser().UserId);
    }

    public async void UpdatePlayerStats(string uuid)
    {
        PlayerStats playerStats = await fbMgr.GetPlayerStats(uuid);

        if (playerStats != null)
        {

            Debug.Log("playerstats... : " + playerStats.PlayerStatsToJson());

            playerXP.text = playerStats.xp + "xp";
            playerTimeSpent.text = playerStats.totalTimeSpent + "secs";
            playerHighScore.text = playerStats.highscore.ToString();
            playerLastPlayed.text = UnixToDateTime(playerStats.updatedOn);

        }
        else
        {
            ResetStatsUI();
        }

        playerName.text = auth.GetCurrentUserDisplayName();
    }

    public void ResetStatsUI()
    {
        playerXP.text = "0 XP";
        playerTimeSpent.text = "0";
        playerHighScore.text = "0";
        playerLastPlayed.text = "0";
    }

    public string UnixToDateTime(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        DateTime dateTime = dateTimeOffset.LocalDateTime;

        return dateTime.ToString("dd MMM yyyy");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
    }

}
