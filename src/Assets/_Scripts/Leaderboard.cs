using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class MyRandom
{
    public static string RandomString(int length)
    {
        const string pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string randomString="";

        for (int i = 0; i < length; i++)
        {
            randomString += pool[Random.Range( 0, pool.Length)];
        }

        return randomString;
    }
}

public class Leaderboard : MonoBehaviour {

    string playerName = "";

    enum gameState {
        waiting,
        running,
        enterscore,
        leaderboard
    };

    gameState gs;


    // Reference to the dreamloLeaderboard prefab in the scene

    dreamloLeaderBoard dl;

    public GameController gameController;
    public GameObject DataEntryPanel;
    public GameObject highScorePanel;
    public Text NameText;
    public Text ListText;

    IEnumerator CheckListReturn()
    {
        string listString;
        do
        {
            Debug.Log("loading...");
            listString = "";
            yield return new WaitForSeconds(0.1f);
            List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow();
            if (scoreList != null)
            {
                int maxToDisplay = 10;
                int count = 0;
                
                foreach (dreamloLeaderBoard.Score currentScore in scoreList)
                {
                    count++;
                    listString += currentScore.shortText.PadRight(12);
                    listString += currentScore.score.ToString().PadLeft(12);
                    listString += "\n";
                    if (count >= maxToDisplay) break;
                }
                ListText.text = listString;
            }
        } while (listString.Trim() == "");
    }

    IEnumerator CheckHighScore()
    {
        bool rec = false;
        float stopTime = Time.time + 4.0f;  //gives up after 4 seconds, assume empty database and allow entry;
        do
        {
            Debug.Log("Polling Highscore...");
            yield return new WaitForSeconds(0.1f);
            if (Time.time > stopTime)
            {
                DataEntryPanel.SetActive(true);
                yield break;
            }
            List<dreamloLeaderBoard.Score> scoreList = dl.ToListLowToHigh();
            if (scoreList.Count > 0)
            {
                rec = true;
                Debug.Log(scoreList.Count);
                if (scoreList[0].score < GameController.GetScore() || scoreList.Count<10)
                {
                    DataEntryPanel.SetActive(true);
                }
                else
                {
                    ShowHighScore();
                }
            }
        } while (!rec);
    }

    void OnEnable()
    {
        // get the reference here...
        HideAll();
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
    }

    public void SubmitName()
    {
        playerName = NameText.text.Trim();
        if (playerName != null)
        {
            int totalScore = GameController.GetScore();
            dl.AddScore(MyRandom.RandomString(10), totalScore, 0, playerName);
            Invoke("ShowHighScore", 1);
            DataEntryPanel.SetActive(false);
        }
    }

    public void ShowHighScore()
    {
        highScorePanel.SetActive(true);
        ListText.text = "(loading...)";
        dl.LoadScores();
        StartCoroutine(CheckListReturn());
        gameController.ShowRestart();
    }

    public void ShowDataEntry()
    {
        dl.LoadScores();
        StartCoroutine(CheckHighScore());
    }

    public void HideAll()
    {
        DataEntryPanel.SetActive(false);
        highScorePanel.SetActive(false);
    }

	
}
