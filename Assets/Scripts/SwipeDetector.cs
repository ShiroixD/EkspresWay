using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 _fingerDownPosition;
    private Vector2 _fingerUpPosition;
    private bool _fingerPushed = false;
    public bool DetectSwipeOnlyAfterRelease = false;
    public float MinDistanceForSwipe = 10f;
    public float SwipeTimeFrame = 0.1f;
    public Player PlayerCharacter;

    void Start()
    {

    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                _fingerUpPosition = touch.position;
                _fingerDownPosition = touch.position;
                _fingerPushed = true;
                StartCoroutine(SwipeTimer());
            }

            //if (!DetectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            //{
            //    _fingerDownPosition = touch.position;
            //    DetectSwipe();
            //}

            if (touch.phase == TouchPhase.Ended && _fingerPushed)
            {
                //_fingerDownPosition = touch.position;
                _fingerPushed = false;
                PlayerCharacter.ReturnToNormal();
                //DetectSwipe();
            }
        }
    }

    private IEnumerator SwipeTimer()
    {
        float currentFrameTime = 0;
        while (_fingerPushed)
        { 
            if (currentFrameTime >= SwipeTimeFrame)
            {
                _fingerDownPosition = Input.touches[Input.touches.Length - 1].position;
                _fingerPushed = false;
                DetectSwipe();
            }
            else
            {
                currentFrameTime += Time.deltaTime;
            }
        }
        yield return new WaitForSeconds(0);
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = _fingerDownPosition.y - _fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            _fingerUpPosition = _fingerDownPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > MinDistanceForSwipe || HorizontalMovementDistance() > MinDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
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
    Up,
    Down,
    Left,
    Right,

}