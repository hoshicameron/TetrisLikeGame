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
    [SerializeField] private Button holdButton;
    [SerializeField] private Button gameOverQuitButton;
    [SerializeField] private Button pauseQuitButton;
    [Header("UI Icons")]
    [SerializeField] private IconToggle musicIconToggle;
    [SerializeField] private IconToggle sfxIconToggle;
    [SerializeField] private IconToggle rotateIconToggle;
    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI linesText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        EventHandler.GameOverEvent+=GameController_OnGameOver;
        EventHandler.RotateButtonPressed += ChangeRotationDirection;
        EventHandler.PauseButtonPressed += TogglePauseGame;
        EventHandler.UpdateScoreEvent += UpdateScoreEvent;
    }



    private void OnDisable()
    {
        EventHandler.GameOverEvent-=GameController_OnGameOver;
        EventHandler.RotateButtonPressed -= ChangeRotationDirection;
        EventHandler.PauseButtonPressed -= TogglePauseGame;
        EventHandler.UpdateScoreEvent -= UpdateScoreEvent;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

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

        gameOverPanelRestartButton.onClick.AddListener(Restart);

        pauseButton.onClick.AddListener(TogglePauseGame);

        pausePanelRestartButton.onClick.AddListener(Restart);

        resumeButton.onClick.AddListener(TogglePauseGame);

        rotateButton.onClick.AddListener(ChangeRotationDirection);

        holdButton.onClick.AddListener(() =>
        {
            GameController.Instance.Hold();
        });

        gameOverQuitButton.onClick.AddListener(ExitGame);

        pauseQuitButton.onClick.AddListener(ExitGame);
    }

    private void UpdateScoreEvent(int score, int lines, int level)
    {
        scoreText.SetText(PadZer0(score,5));
        levelText.SetText(level.ToString());
        linesText.SetText(lines.ToString());
    }

    private void TogglePauseGame()
    {
        GameController.Instance.TogglePause();
        pausePanel.SetActive(!pausePanel.activeInHierarchy);
        AudioManager.Instance.ToggleMusicVolume(pausePanel.activeInHierarchy);
    }

    private void ChangeRotationDirection()
    {
        GameController.Instance.ToggleRotDirection(out bool enable);
        rotateIconToggle.Toggle(enable);
    }

    private void GameController_OnGameOver()
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

    private void ExitGame()
    {
        Application.Quit();
    }

}
