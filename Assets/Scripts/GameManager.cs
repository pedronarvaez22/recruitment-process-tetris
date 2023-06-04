using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }

    [Header("Current Game State")]
    public GameStates currentGameState;

    [Header("UI Manager Reference")]
    [SerializeField]
    private UIManager uiManager;

    [Header("Block Spawner Reference")]
    [SerializeField]
    private BlockSpawner blockSpawner;
    public BlockSpawner BlockSpawner => blockSpawner;

    [Header("Instructions")]
    [SerializeField]
    private GameObject instructionsGo;

    [Header("Game Over Bool")]
    public bool gameOver;

    public int score;

    public bool isPaused;

    public enum GameStates
    {
        Ready,
        Playing,
        GameOver
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        gameOver = false;
        score = 0;
        ScoreUpdate(score);
        StartCoroutine(ChangeGameStates(GameStates.Ready));
    }

    public void ScoreUpdate(int amount)
    {
        score += amount;
        uiManager.UpdateScoreText(score);
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            uiManager.PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            uiManager.PausePanel.SetActive(false);
        }
    }

    public IEnumerator ChangeGameStates(GameStates newState)
    {
        currentGameState = newState;
        switch(currentGameState)
        {
            case GameStates.Ready:
                instructionsGo.SetActive(true);
                break;
            case GameStates.Playing:
                blockSpawner.SpawnNewBlock();
                instructionsGo.SetActive(false);
                break;
            case GameStates.GameOver:
                gameOver = true;
                instructionsGo.SetActive(false);
                AudioManager.Instance.StopMx();
                yield return new WaitForSeconds(.2f);
                AudioManager.Instance.PlayFX(2);
                uiManager.GameOverPanel.SetActive(true);
                break;
        }
    }
}
