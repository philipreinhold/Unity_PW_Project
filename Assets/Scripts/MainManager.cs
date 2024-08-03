using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText; // Dieses Text-Element zeigt sowohl den Spielernamen als auch den Highscore an.
    public GameObject GameOverText;
    public GameObject gameUI; // GameObject, das die Spiel-UI enthält

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;

    private int highScore;
    private string highScorePlayer;

    // Start is called before the first frame update
    void Start()
    {
        gameUI.SetActive(false); // Verstecke die Spiel-UI beim Start

        LoadHighScore();
        UpdateHighScoreUI();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started && !m_GameOver)
        {
            return;
        }

        if (m_Started && Input.GetKeyDown(KeyCode.Space))
        {
            m_Started = false;
            gameUI.SetActive(true); // Zeige die Spiel-UI an, wenn das Spiel startet

            float randomDirection = Random.Range(-1.0f, 1.0f);
            Vector3 forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            Ball.transform.SetParent(null);
            Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }

        if (m_GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartGame()
    {
        m_Started = true;
        gameUI.SetActive(true); // Zeige die Spiel-UI an, wenn das Spiel startet
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > highScore)
        {
            highScore = m_Points;
            highScorePlayer = GameData.PlayerName;
            SaveHighScore();
            UpdateHighScoreUI();
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScorePlayer = PlayerPrefs.GetString("HighScorePlayer", "None");
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetString("HighScorePlayer", highScorePlayer);
        PlayerPrefs.Save();
    }

    private void UpdateHighScoreUI()
    {
        if (HighScoreText != null)
        {
            HighScoreText.text = $"Best Score: {highScorePlayer} - {highScore}\nCurrent Player: {GameData.PlayerName} - {m_Points}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
