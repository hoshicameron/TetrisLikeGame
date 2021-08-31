using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public event EventHandler OnGameOver;
    public event EventHandler OnRotButtonPressed;
    public event EventHandler OnPauseButtonPressed;
    // Delay on move shape down
    [SerializeField] private float dropInterval=0.5f;

    // Reference to boaard
    private Board board;
    // Reference to spawner
    private Spawner spawner;
    // Reference to scoreManager
    private ScoreManager scoreManager;
    // Currently active shape
    private Shape activeShape;
    // TimeToDrop to move shape with interval
    private float timeToDrop;


    // Delay on move shape with player input
    [Range(0.02f,1f)]
    [SerializeField]private float keyRepeatRateLeftRight = 0.15f;
    private float timeToNextKeyLeftRight;
    [Range(0.01f,1f)]
    [SerializeField]private float keyRepeatRateDown = 0.01f;
    private float timeToNextKeyDown;
    [Range(0.02f,1f)]
    [SerializeField]private float keyRepeatRateRotate = 0.25f;
    private float timeToNextKeyRotate;

    private bool gameOver = false;
    private bool paused = false;

    private bool clockwise = true;

    private float dropIntervalModed;



    private void Start()
    {
        //Find spawner and board
        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
        scoreManager = FindObjectOfType<ScoreManager>();

        timeToDrop = Time.time + dropInterval;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

        //If board or spawner are not find the show a error message
        if (board == null)
        {
            Debug.Log("Warning! There is no game board defined!");
        }
        if(scoreManager==null)
        {
            Debug.Log("Warning! There is no score manager defined!");
        }

        if (spawner == null)
        {
            Debug.Log("Warning! There is no game spawner defined!");
        } else
        {
            spawner.transform.position = VectorF.Round(spawner.transform.position);
            if (activeShape == null)
            {
                activeShape = spawner.SpawnShape();
            }
        }
        // Play main music
        AudioManager.Instance.PlayMusic();

        dropIntervalModed = dropInterval;

    }

    private void Update()
    {
        //if we don't have a spawner or game board just don't run the game
        if (board == null || spawner == null || activeShape==null || gameOver)
        {
            return;
        }
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > timeToNextKeyLeftRight)
        {
            activeShape.MoveRight();
            timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
            if (!board.IsValidPosition(activeShape))
            {
                activeShape.MoveLeft();
                AudioManager.Instance.PlayErrorAudioClip();
            }
            else
            {
                AudioManager.Instance.PlayMoveAudioClip();
            }
        }

        else if (Input.GetButton("MoveLeft") && Time.time > timeToNextKeyLeftRight)
        {
            activeShape.MoveLeft();
            timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
            if (!board.IsValidPosition(activeShape))
            {
                activeShape.MoveRight();
                AudioManager.Instance.PlayErrorAudioClip();
            } else
            {
                AudioManager.Instance.PlayMoveAudioClip();
            }
        }

        else if (Input.GetButtonDown("Rotate") && Time.time > timeToNextKeyRotate)
        {
            activeShape.RotateClockwise(clockwise);

            timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
            if (!board.IsValidPosition(activeShape))
            {
                activeShape.RotateClockwise(!clockwise);
                AudioManager.Instance.PlayErrorAudioClip();
            } else
            {
                AudioManager.Instance.PlayMoveAudioClip();
            }
        } else if (Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown || Time.time>timeToDrop)
        {
            timeToNextKeyDown = Time.time + keyRepeatRateDown;
            timeToDrop = Time.time + dropIntervalModed;

            activeShape.MoveDown();

            if (!board.IsValidPosition(activeShape))
            {
                if (board.IsOverLimit(activeShape))
                {
                    GameOver();
                } else
                {
                    LandShape();
                }
            } else
            {
                AudioManager.Instance.PlayMoveAudioClip();
            }
        }
        else if (Input.GetButtonDown("ToggleRot"))
        {
            OnRotButtonPressed?.Invoke(this,EventArgs.Empty);
        }
        else if (Input.GetButtonDown("Pause"))
        {
            OnPauseButtonPressed?.Invoke(this,EventArgs.Empty);
        }
    }

    private void GameOver()
    {
        activeShape.MoveUp();
        gameOver = true;
        AudioManager.Instance.PlayGameOverAudioClip();
        AudioManager.Instance.PlayVocalGameOverClip();
        OnGameOver?.Invoke(this, EventArgs.Empty);
        //Debug.LogWarning("The shape pass the board boundaries! Game Over...");
    }

    private void LandShape()
    {
        timeToNextKeyDown = Time.time;
        timeToNextKeyLeftRight = Time.time ;
        timeToNextKeyRotate = Time.time ;
        // Shape land here
        activeShape.MoveUp();
        // Add shape to the grid
        board.StoreShapeInGrid(activeShape);
        // Spawn new shape
        activeShape = spawner.SpawnShape();

        board.ClearAllRows();
        AudioManager.Instance.PlayDropAudioClip();

        if (board.CompletedRows > 0)
        {
            scoreManager.ScoreLines(board.CompletedRows);

            if (scoreManager.DidLevelUp)
            {
                //Clear one or more rows and levelUp
                AudioManager.Instance.PlayLevelUpVocalAudioClip();
                //Update game speed
                dropIntervalModed =
                    Mathf.Clamp(dropInterval - (((float) scoreManager.GetLevel - 1) * 0.05f), 0.05f, 1f);
            } else
            {
                if (board.CompletedRows > 1)
                {
                    //Clear more than one row
                    AudioManager.Instance.PlayVocalClip();
                }
            }
            //Clear one row
            AudioManager.Instance.PlayClearRowAudioClip();
        }
    }

    public void ToggleRotDirection(out bool isEnable)
    {
        clockwise = !clockwise;
        isEnable = clockwise;
    }

    public void TogglePause()
    {
        if (gameOver)    return;

        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
    }
}
