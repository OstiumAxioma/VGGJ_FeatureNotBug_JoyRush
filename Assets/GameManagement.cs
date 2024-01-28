using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;
    public bool playerActive = true;
    public bool isGameOver = false;

    private AudioSource audioSource;

    [ContextMenu("Increase Score")]
    public void updateScore(int score)
    {
        if (score >= 0)
        {
            playerScore = score;
            scoreText.text = playerScore.ToString();
        } else
        {
            Debug.LogWarning("Abnormal score value: " + score.ToString());
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene("Main_Scene");
        isGameOver = false;
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
        playerActive = false;
        isGameOver = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
