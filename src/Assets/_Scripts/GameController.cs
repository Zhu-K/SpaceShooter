using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public Vector2 hazardRange;
    public Vector2 spawnWait;
    public float startWait;
    public float waveWait;
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameoverText;
    public Leaderboard leaderboard;

    private static int score;
    private bool gameOver;
    private bool restart;

    public static GameController FindController()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            return gameControllerObject.GetComponent<GameController>();
        }
        else return null;
    }

    public static int GetScore()
    {
        return score;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }



    public void AddScore(int newScoreValue)
    {
        if (gameOver) return;
        score += newScoreValue;
        UpdateScore();
    }

    public void GameOver()
    {
        gameoverText.text = "GAME OVER\n\nSCORE: " + score;

        gameOver = true;
    }

    public void ShowRestart()
    {
        restartText.text = "Press 'R' to Restart";
        restart = true;
    }

    public void StartGame()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            int hazardCount = Mathf.CeilToInt(Random.Range(hazardRange.x, hazardRange.y));
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(Random.Range(spawnWait.x, spawnWait.y));
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                leaderboard.ShowDataEntry();
                break;
            }
        }
    }

	// Use this for initialization
	void Start () {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameoverText.text = "";
        score = 0;
        UpdateScore();
        StartGame();
	}

    private void Update()
    {
        if(restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
