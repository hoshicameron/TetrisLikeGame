using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button restartButton;
    private GameController gameController;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        gameController = FindObjectOfType<GameController>();
        gameController.OnGameOver+=GameController_OnGameOver;

        musicButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleMusic();
        });

        sfxButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.ToggleSFX();
        });

        restartButton.onClick.AddListener(()=>
        {
            Restart();
        });
    }



    private void GameController_OnGameOver(object sender, EventArgs e)
    {
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Debug.Log("Restarting...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
