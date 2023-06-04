using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text finalScoreText;
    [SerializeField]
    private Button startButton;

    [Header("Panels")]
    [SerializeField]
    private GameObject pausePanel;
    public GameObject PausePanel => pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    public GameObject GameOverPanel => gameOverPanel;

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
        finalScoreText.text = score.ToString();
    }

    public void ButtonPressed(string buttonName)
    {
        switch(buttonName)
        {
            case "Pause":
                GameManager.Instance.isPaused = true;
                GameManager.Instance.PauseGame();
                break;
            case "StartGame":
                StartCoroutine(GameManager.Instance.ChangeGameStates(GameManager.GameStates.Playing));
                startButton.gameObject.SetActive(false);
                break;
            case "MainMenu":
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenuScene");
                break;
            case "Resume":
                GameManager.Instance.isPaused = false;
                GameManager.Instance.PauseGame();
                break;
            case "TryAgain":
                Time.timeScale = 1;
                SceneManager.LoadScene("GameScene");
                break;
        }
    }
}
