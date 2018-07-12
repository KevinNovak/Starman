using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [Header("Score Text")]
    public Text scoreText;
    public Text highscoreText;

    [Header("Score Per Second")]
    public float scorePerSecond = 10f;
    public bool active = true;

    private int highscore;
    private float score = 0;

    // Use this for initialization
    void Start()
    {
        scoreText.text = score.ToString();

        highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "Best: " + formatScore(highscore);
    }

    void Update()
    {
        if (active)
        {
            var points = scorePerSecond * Time.deltaTime;
            Add(points);
        }
    }

    public void Add(float points)
    {
        score += points;
        scoreText.text = formatScore(score);
    }

    public void EndGame()
    {
        var newScore = Mathf.RoundToInt(score);
        if (newScore > highscore)
        {
            highscore = newScore;
            highscoreText.text = "Best: " + formatScore(highscore);
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    private string formatScore(int score)
    {
        return (string.Format("{0:n0}", score));
    }

    private string formatScore(float score)
    {
        var scoreRounded = Mathf.RoundToInt(score);
        return formatScore(scoreRounded);
    }
}
