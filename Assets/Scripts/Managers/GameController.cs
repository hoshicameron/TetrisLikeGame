using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public event EventHandler OnGameOver;
    // Delay on move shape down
    [SerializeField] private float dropInterval=0.5f;

    // Reference to boaard
    private Board board;
    // Reference to spawner
    private Spawner spawner;

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



    private void Start()
    {
        //Find spawner and board
        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();

        timeToDrop = Time.time + dropInterval;
        timeToNextKeyDown = Time.time + keyRepeatRateDown;
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

        //If board or spawner are not find the show a error message
        if (board == null)
        {
            Debug.Log("Warning! There is no game board defined!");
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
            activeShape.RotateRight();

            timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
            if (!board.IsValidPosition(activeShape))
            {
                activeShape.RotateLeft();
                AudioManager.Instance.PlayErrorAudioClip();
            } else
            {
                AudioManager.Instance.PlayMoveAudioClip();
            }
        } else if (Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown || Time.time>timeToDrop)
        {
            timeToNextKeyDown = Time.time + keyRepeatRateDown;
            timeToDrop = Time.time + dropInterval;

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
            if (board.CompletedRows > 1)
            {
                AudioManager.Instance.PlayVocalClip();
            }
            AudioManager.Instance.PlayClearRowAudioClip();
        }
    }
}
