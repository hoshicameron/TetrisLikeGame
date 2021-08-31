using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UIPanels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [Header("UI Buttons")]
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button gameOverPanelRestartButton;
    [SerializeField] private Button pausePanelRestartButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [Header("UI Icons")]
    [SerializeField] private IconToggle musicIconToggle;
    [SerializeField] private IconToggle sfxIconToggle;
    [SerializeField] private IconToggle rotateIconToggle;
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI linesText;
    [SerializeField] private TextMeshProUGUI levelText;


    private GameController gameController;
    private ScoreManager scoreManager;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameController = FindObjectOfType<GameController>();
        scoreManager = FindObjectOfType<ScoreManager>();

        if (gameController != null)
        {
            gameController.OnGameOver+=GameController_OnGameOver;
            gameController.OnRotButtonPressed += (sender, args) => ChangeRotationDirection();
            gameController.OnPauseButtonPressed += (sender, args) => TogglePauseGame();
        }

        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdated+=ScoreManager_OnScoreUpdated;
        }



        musicButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleMusic(out bool enable);
            musicIconToggle.Toggle(enable);
        });

        sfxButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleSFX(out bool enable);
            sfxIconToggle.Toggle(enable);
        });

        gameOverPanelRestartButton.onClick.AddListener(()=>
        {
            Restart();
        });

        pauseButton.onClick.AddListener(TogglePauseGame);

        pausePanelRestartButton.onClick.AddListener(() =>
        {
            Restart();
        });

        resumeButton.onClick.AddListener(TogglePauseGame);

        rotateButton.onClick.AddListener(ChangeRotationDirection);
    }

    private void ScoreManager_OnScoreUpdated(object sender, ScoreManager.ScoreEventArgs e)
    {
        scoreText.SetText(PadZer0(e.m_score,5));
        levelText.SetText(e.m_Level.ToString());
        linesText.SetText(e.m_lines.ToString());

    }

    private void TogglePauseGame()
    {
        gameController.TogglePause();
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        AudioManager.Instance.ToggleMusicVolume(pausePanel.activeInHierarchy);
    }

    private void ChangeRotationDirection()
    {
        gameController.ToggleRotDirection(out bool enable);
        rotateIconToggle.Toggle(enable);
    }

    private void GameController_OnGameOver(object sender, EventArgs e)
    {
        gameOverPanel.SetActive(true);
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private string PadZer0(int n, int padDigits)
    {
        string nString = n.ToString();

        while (nString.Length<padDigits)
        {
            nString = $"0{nString}";
        }

        return nString;
    }

}
