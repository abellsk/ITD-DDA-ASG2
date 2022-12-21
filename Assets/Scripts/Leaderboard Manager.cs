using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{

    public MyDatabase fbManager;
    public GameObject rowPrefab;
    public Transform tableContent;

    // Start is called before the first frame update
    void Start()
    {
        GetLeaderboard();
    }

    public void GetLeaderboard()
    {
        UpdateLeaderboardUI();
    }

    public async void UpdateLeaderboardUI()
    {
        var leaderboardlist = await fbManager.GetLeaderboard(6);
        int rankCounter = 1;

        //clear all leaderboard entry in ui so only showing data from firebase
        foreach (Transform item in tableContent)
        {
            Destroy(item.gameObject);
        }

        //create prefabs of our rows
        //assign each value from list to the prefab text content
        foreach (Leaderboard lb in leaderboardlist)
        {
            Debug.LogFormat("Leaderboard: Rank {0} Playername {1} Highscore {2}", rankCounter, lb.userName, lb.highScore);

            //create prefabs in the position of tableContent
            GameObject entry = Instantiate(rowPrefab, tableContent);
            TextMeshProUGUI[] leaderboardDetails = entry.GetComponentsInChildren<TextMeshProUGUI>();
            leaderboardDetails[0].text = rankCounter.ToString();
            leaderboardDetails[1].text = lb.userName;
            leaderboardDetails[2].text = lb.highScore.ToString();

            rankCounter++;
        }
    }

    public void GoToGameMenu()
    {
        SceneManager.LoadScene(1);
    }

}
