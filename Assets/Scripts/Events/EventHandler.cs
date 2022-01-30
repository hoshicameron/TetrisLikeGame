using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action GameOverEvent;

    public static void CallGameOvrEvent()
    {
        GameOverEvent?.Invoke();
    }

    public static event Action RotateButtonPressed;

    public static void CallRotateButtonPressed()
    {
        RotateButtonPressed?.Invoke();
    }

    public static event Action PauseButtonPressed;

    public static void CallPauseButtonPressed()
    {
        PauseButtonPressed?.Invoke();
    }

    public static event Action<Vector2> SwipeEvent;

    public static void CallSwipeEvent(Vector2 swipe)
    {
        SwipeEvent?.Invoke(swipe);
    }

    public static event Action<Vector2> SwipeEndEvent;

    public static void CallSwipeEndEvent(Vector2 swipe)
    {
        SwipeEndEvent?.Invoke(swipe);
    }

    public static event Action<int, int, int> UpdateScoreEvent;

    public static void CallUpdateScoreEvent(int score, int lines, int level)
    {
        UpdateScoreEvent?.Invoke(score,lines,level);
    }
}
