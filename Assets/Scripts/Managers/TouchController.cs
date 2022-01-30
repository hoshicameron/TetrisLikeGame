using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TouchController : SingletonMonoBehaviour<TouchController>
{
    /*public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler SwipeEndEvent;*/

    private Vector2 touchMovement;
    private int minSwipeDistance = 20;

    public TextMeshProUGUI diagnosticText1;
    public TextMeshProUGUI diagnosticText2;

    public bool useDiagnostic = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Diagnostic(string.Empty, string.Empty);
    }

    void Diagnostic(string text1, string text2)
    {
        diagnosticText1.gameObject.SetActive(useDiagnostic);
        diagnosticText2.gameObject.SetActive(useDiagnostic);
        if(diagnosticText1!=null && diagnosticText2!=null)
        {
            diagnosticText1.SetText(text1);
            diagnosticText2.SetText(text2);
        }
    }

    string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = string.Empty;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x > 0) ? "right" : "left";
        } else
        {
            direction = (swipeMovement.y > 0) ? "up" : "down";
        }

        return direction;
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                touchMovement=Vector2.zero;
                Diagnostic(string.Empty, string.Empty);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // keep total of finger displacement;
                touchMovement += touch.deltaPosition;

                if (touchMovement.magnitude > minSwipeDistance)
                {
                    EventHandler.CallSwipeEvent(touchMovement);
                    Diagnostic("Swipe detected", $"{touchMovement} {SwipeDiagnostic(touchMovement)}");
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                EventHandler.CallSwipeEndEvent(touchMovement);
            }
        }
    }
}
