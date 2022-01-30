using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{


    // Delay on move shape down
    [SerializeField] private float dropInterval=0.5f;

    // Reference to board
    private Board board;
    // Reference to spawner
    private Spawner spawner;
    // Currently active shape
    private Shape activeShape;
    // TimeToDrop to move shape with interval
    private float timeToDrop;

    // Ghost for Visualization
    private Ghost ghost;

    // holder reference
    private Holder holder;


    // Delay on move shape with player input
    [Range(0.02f,1f)]
    [SerializeField]private float keyRepeatRateLeftRight = 0.15f;
    private float timeToNextKeyLeftRight;
    [Range(0.01f,1f)]
    [SerializeField]private float keyRepeatRateDown = 0.01f;
    private float timeToNextKeyDown;
    [Range(0.02f,1f)]
    [SerializeField]private float keyRepeatRateRotate = 0.25f;

    [SerializeField] private ParticlePlayer gameOverFX;
    private float timeToNextKeyRotate;

    private bool gameOver = false;
    private bool paused = false;

    private bool clockwise = true;

    private float dropIntervalModed;

    enum Direction
    {
        none,
        left,
        right,
        up,
        down
    };

    private Direction swipeDirection = Direction.none;
    private Direction swipeEndDirection = Direction.none;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //Find spawner and board
        board = FindObjectOfType<Board>();
        spawner = FindObjectOfType<Spawner>();
        ghost = FindObjectOfType<Ghost>();
        holder = FindObjectOfType<Holder>();

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

        dropIntervalModed = dropInterval;

    }

    private void OnEnable()
    {
        EventHandler.SwipeEvent += SwipeHandler;
        EventHandler.SwipeEndEvent += SwipeEndHandler;
    }

    private void OnDisable()
    {
        EventHandler.SwipeEvent -= SwipeHandler;
        EventHandler.SwipeEndEvent -= SwipeEndHandler;
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

    private void LateUpdate()
    {
        if (ghost != null)
        {
            ghost.DrawGhost(activeShape,board);
        }
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > timeToNextKeyLeftRight)
        {
            MoveRight();
        }

        else if (Input.GetButton("MoveLeft") && Time.time > timeToNextKeyLeftRight)
        {
            MoveLeft();
        }

        else if (Input.GetButtonDown("Rotate") && Time.time > timeToNextKeyRotate)
        {
            Rotate();
        } else if (Input.GetButton("MoveDown") && Time.time > timeToNextKeyDown || Time.time>timeToDrop)
        {
            MoveDown();
        }
        // Swipe to right
        else if ((swipeDirection==Direction.right && Time.time>timeToNextKeyLeftRight)|| swipeEndDirection==Direction.right)
        {
            MoveRight();
            swipeDirection = Direction.none;
            swipeEndDirection = Direction.none;
        }
        // Swipe to Left
        else if ((swipeDirection==Direction.left && Time.time>timeToNextKeyLeftRight)|| swipeEndDirection==Direction.left)
        {
            MoveLeft();
            swipeDirection = Direction.none;
            swipeEndDirection = Direction.none;
        }
        // Swipe to up when touch ended
        else if (swipeEndDirection==Direction.up)
        {
            Rotate();
            swipeEndDirection = Direction.none;

        }
        // Swipe to down while wipe down
        else if (swipeDirection==Direction.down && Time.time>timeToNextKeyDown)
        {
            MoveDown();
            swipeDirection = Direction.none;

        }

        else if (Input.GetButtonDown("ToggleRot"))
        {
            EventHandler.CallRotateButtonPressed();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            EventHandler.CallPauseButtonPressed();
        }
        else if (Input.GetButtonDown("Hold"))
        {
            Hold();
        }
    }

    private void MoveDown()
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

    private void Rotate()
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
    }

    private void MoveLeft()
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

    private void MoveRight()
    {
        activeShape.MoveRight();
        timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
        if (!board.IsValidPosition(activeShape))
        {
            activeShape.MoveLeft();
            AudioManager.Instance.PlayErrorAudioClip();
        } else
        {
            AudioManager.Instance.PlayMoveAudioClip();
        }
    }

    private void GameOver()
    {
        activeShape.MoveUp();
        gameOver = true;
        AudioManager.Instance.PlayGameOverAudioClip();
        AudioManager.Instance.PlayVocalGameOverClip();

        StartCoroutine(nameof(GameOverRoutine));
    }

    IEnumerator GameOverRoutine()
    {
        if (gameOverFX!=null)
        {
            gameOverFX.Play();
        }
        EventHandler.CallGameOvrEvent();
        yield return new WaitForSeconds(0.5f);
    }

    private void LandShape()
    {
        // set the all input delay to zero, so there is no delay when new shape spawns
        timeToNextKeyDown = Time.time;
        timeToNextKeyLeftRight = Time.time ;
        timeToNextKeyRotate = Time.time ;
        // Shape land here
        activeShape.MoveUp();
        // Add shape to the grid
        board.StoreShapeInGrid(activeShape);

        // spawn land shape effect
        activeShape.LandShapeFX();
        // Destroy ghost shape
        if (ghost != null)
        {
            ghost.Reset();
        }

        if (holder)
        {
            holder.CanRelease = true;
        }

        // Spawn new shape
        activeShape = spawner.SpawnShape();

        // remove completed rows from the board if we have any
        board.StartCoroutine(nameof(Board.ClearAllRows));
        AudioManager.Instance.PlayDropAudioClip();

        if (board.CompletedRows > 0)
        {
            ScoreManager.Instance.ScoreLines(board.CompletedRows);

            if (ScoreManager.Instance.DidLevelUp)
            {
                //Clear one or more rows and levelUp
                AudioManager.Instance.PlayLevelUpVocalAudioClip();
                //Update game speed
                dropIntervalModed =
                    Mathf.Clamp(dropInterval - (((float) ScoreManager.Instance.GetLevel - 1) * 0.05f), 0.05f, 1f);
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

    public void Hold()
    {
        if(holder==null) return;
        if (holder.IsHolderEmpty())
        {
            holder.Catch(activeShape);
            activeShape = spawner.SpawnShape();
            AudioManager.Instance.PlayHoldShapeAudioClip();
        } else
        {
            if (holder.CanRelease)
            {
                Shape temp = activeShape;
                activeShape = holder.Release();
                activeShape.transform.position = spawner.transform.position;
                holder.Catch(temp);
                AudioManager.Instance.PlayHoldShapeAudioClip();

            } else
            {
                Debug.LogWarning("Holder Warning! Wait for cool down");
                AudioManager.Instance.PlayErrorAudioClip();
            }


        }

        if (ghost)
        {
            ghost.Reset();
        }

    }

    void SwipeHandler(Vector2 swipeMovement)
    {
        swipeDirection = GetDirection(swipeMovement);
    }

    void SwipeEndHandler(Vector2 swipeMovement)
    {
        swipeEndDirection = GetDirection(swipeMovement);
    }

    Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.none;
        // Horizontal
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = swipeMovement.x >= 0 ? Direction.right : Direction.left;
        }
        // Vertical
        else
        {
            swipeDir = swipeMovement.y >= 0 ? Direction.up : Direction.down;
        }

        return swipeDir;
    }
}
