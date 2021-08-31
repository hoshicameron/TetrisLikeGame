using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button gameOverPanelRestartButton;
    [SerializeField] private Button pausePanelRestartButton;
    [SerializeField] private Button rotateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    [SerializeField] private IconToggle musicIconToggle;
    [SerializeField] private IconToggle sfxIconToggle;
    [SerializeField] private IconToggle rotateIconToggle;
    private GameController gameController;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameController = FindObjectOfType<GameController>();
        gameController.OnGameOver+=GameController_OnGameOver;
        gameController.OnRotButtonPressed += (sender, args) => ChangeRotationDirection();
        gameController.OnPauseButtonPressed += (sender, args) => TogglePauseGame();

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


}
