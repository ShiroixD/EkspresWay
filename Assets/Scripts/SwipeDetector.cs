using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    private bool _fingerPushed = false;
    public float SwipeTimeFrame = 0.1f;
    public Player PlayerCharacter;

    void Start()
    {
        Input.multiTouchEnabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(SwipeTimer());
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _fingerPushed = false;
                PlayerCharacter.ReturnToNormal();
            }
        }    
    }

    private IEnumerator SwipeTimer()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _fingerUpPosition = touch.position;
            _fingerDownPosition = touch.position;
            _fingerPushed = true;
            float currentFrameTime = 0;

            while (_fingerPushed && Input.touchCount > 0)
            {
                if (currentFrameTime >= SwipeTimeFrame)
                {
                    _fingerDownPosition = touch.position;
                    _fingerPushed = false;
                    if (touch.phase == TouchPhase.Moved)
                        DetectSwipe();
                    break;
                }
                else
                {
                    currentFrameTime += Time.deltaTime;
                }

                yield return new WaitForSeconds(0);
            }
        }

        yield return new WaitForSeconds(0);
    }

    private void DetectSwipe()
    {
        SwipeDirection direction = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        SendSwipe(direction);
        _fingerUpPosition = _fingerDownPosition;
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = _fingerDownPosition,
            EndPosition = _fingerUpPosition
        };
        PlayerCharacter.TiltToSide(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Left,
    Right,
}